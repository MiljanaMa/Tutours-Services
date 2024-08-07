package repo

import (
	"context"
	"fmt"
	"go.mongodb.org/mongo-driver/bson"
	"go.mongodb.org/mongo-driver/mongo"
	"go.mongodb.org/mongo-driver/mongo/readpref"
	"log"
	"time"
	"tours/model"
	"tours/model/helper"
)

type TourRepository struct {
	Cli    *mongo.Client
	Logger *log.Logger
}

// Check database connection
func (pr *TourRepository) Ping() {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Second)
	defer cancel()

	// Check connection -> if no error, connection is established
	err := pr.Cli.Ping(ctx, readpref.Primary())
	if err != nil {
		pr.Logger.Println(err)
	}

	// Print available databases
	databases, err := pr.Cli.ListDatabaseNames(ctx, bson.M{})
	if err != nil {
		pr.Logger.Println(err)
	}
	fmt.Println(databases)
}

func (repo *TourRepository) Create(tour *model.Tour) (model.Tour, error) {
	tour.Tags = helper.ArrayString{"aaa"}
	err := tour.Validate()
	if err != nil {
		return *tour, err
	}
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Second)
	defer cancel()

	toursCollection := repo.getCollection()
	counterCollection := repo.getCounter()

	filter := bson.M{"name": "tour"}
	var counter struct {
		Id    int    `bson:"_id"`
		Value int    `bson:"value"`
		Name  string `bson:"name"`
	}
	errCounter := counterCollection.FindOne(ctx, filter).Decode(&counter)
	if errCounter != nil {
		return *tour, errCounter
	}

	currentCounterValue := counter.Value

	tour.Id = currentCounterValue
	_, err = toursCollection.InsertOne(ctx, &tour)
	if err != nil {
		return *tour, err
	}

	newCounterValue := currentCounterValue + 1

	update := bson.M{"$set": bson.M{"value": newCounterValue}}
	_, err = counterCollection.UpdateOne(ctx, filter, update)
	if err != nil {
		return *tour, err
	}
	return *tour, nil
}
func (repo *TourRepository) Update(tour *model.Tour) (model.Tour, error) {
	tourForUpdate, err := repo.GetById(tour.Id)
	if err != nil {
		return *tour, err
	}

	err = tour.Validate()
	if err != nil {
		return *tour, err
	}

	err = tour.ValidateUpdate(&tourForUpdate)
	if err != nil {
		return *tour, err
	}

	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Second)
	defer cancel()

	toursCollection := repo.getCollection()

	filter := bson.M{"_id": tour.Id}
	// Create the update map
	update := bson.M{"$set": bson.M{
		"tags":               tour.Tags,
		"description":        tour.Description,
		"price":              tour.Price,
		"duration":           tour.Duration,
		"difficulty":         tour.Difficulty,
		"distance":           tour.Distance,
		"status_update_time": tour.StatusUpdateTime,
		"name":               tour.Name,
		"status":             tour.Status,
		"transport_type":     tour.TransportType,
	}}

	result, err := toursCollection.UpdateOne(ctx, filter, update)
	repo.Logger.Printf("Documents matched: %v\n", result.MatchedCount)
	repo.Logger.Printf("Documents updated: %v\n", result.ModifiedCount)

	if err != nil {
		repo.Logger.Println(err)
		return model.Tour{}, err
	}
	updatedTour, err := repo.GetById(tour.Id)
	if err != nil {
		repo.Logger.Println("Error fetching updated tour:", err)
		return model.Tour{}, err
	}

	return updatedTour, nil
}
func (repo *TourRepository) Delete(tourId int) error {

	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Second)
	defer cancel()
	toursCollection := repo.getCollection()

	filter := bson.D{{Key: "_id", Value: tourId}}
	result, err := toursCollection.DeleteOne(ctx, filter)
	if err != nil {
		return err
	}
	repo.Logger.Printf("Tours deleted: %v\n", result.DeletedCount)

	//delete related keypoints
	keypointsCollection := repo.getKeypoints()

	filterKp := bson.D{{Key: "tour_id", Value: tourId}}
	result, err = keypointsCollection.DeleteMany(ctx, filterKp)
	if err != nil {
		return err
	}
	repo.Logger.Printf("Keypoints deleted: %v\n", result.DeletedCount)

	//delete related reviews
	reviewsCollection := repo.getReviews()

	filterR := bson.D{{Key: "tour_id", Value: tourId}}
	result, err = reviewsCollection.DeleteMany(ctx, filterR)
	if err != nil {
		return err
	}
	repo.Logger.Printf("Reviews deleted: %v\n", result.DeletedCount)

	return nil
}

func (repo *TourRepository) GetAllByAuthor(limit, page, userId int) ([]model.Tour, error) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Second)
	defer cancel()

	toursCollection := repo.getCollection()
	filter := bson.M{"user_id": userId}

	var tours []model.Tour
	toursCursor, err := toursCollection.Find(ctx, filter)
	if err != nil {
		return nil, err
	}
	defer toursCursor.Close(ctx)
	if err := toursCursor.All(ctx, &tours); err != nil {
		return nil, err
	}

	return tours, nil
}

func (repo *TourRepository) GetAll(limit, page int) ([]model.Tour, error) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Second)
	defer cancel()

	toursCollection := repo.getCollection()

	var tours []model.Tour
	toursCursor, err := toursCollection.Find(ctx, bson.M{})
	if err != nil {
		return nil, err
	}
	if err := toursCursor.All(ctx, &tours); err != nil {
		return nil, err
	}

	return tours, nil
}
func (repo *TourRepository) GetById(id int) (model.Tour, error) {
	ctx, cancel := context.WithTimeout(context.Background(), 55*time.Second)
	defer cancel()

	toursCollection := repo.getCollection()
	filter := bson.M{"_id": id}

	var tour model.Tour
	err := toursCollection.FindOne(ctx, filter).Decode(&tour)
	if err != nil {
		return tour, err
	}

	//get related keypoints
	keypointsCollection := repo.getKeypoints()
	filterKp := bson.M{"tour_id": id}

	var keypoints []model.Keypoint
	keypointsCursor, err := keypointsCollection.Find(ctx, filterKp)
	if err != nil {
		return tour, err
	}
	defer keypointsCursor.Close(ctx)
	if err := keypointsCursor.All(ctx, &keypoints); err != nil {
		return tour, err
	}
	tour.Keypoints = keypoints

	return tour, nil
}
func (tr *TourRepository) getCollection() *mongo.Collection {
	tourDatabase := tr.Cli.Database("tourService")
	toursCollection := tourDatabase.Collection("tours")
	return toursCollection
}
func (tr *TourRepository) getCounter() *mongo.Collection {
	tourDatabase := tr.Cli.Database("tourService")
	counter := tourDatabase.Collection("counter")
	return counter
}
func (tr *TourRepository) getKeypoints() *mongo.Collection {
	tourDatabase := tr.Cli.Database("tourService")
	keypointsCollection := tourDatabase.Collection("keypoints")
	return keypointsCollection
}
func (tr *TourRepository) getReviews() *mongo.Collection {
	reviewDatabase := tr.Cli.Database("tourService")
	reviewCollection := reviewDatabase.Collection("reviews")
	return reviewCollection
}
