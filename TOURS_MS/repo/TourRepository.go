package repo

import (
	"context"
	"fmt"
	"go.mongodb.org/mongo-driver/bson"
	"go.mongodb.org/mongo-driver/bson/primitive"
	"go.mongodb.org/mongo-driver/mongo"
	"go.mongodb.org/mongo-driver/mongo/readpref"
	"gorm.io/gorm"
	"log"
	"reflect"
	"time"
	"tours/model"
)

type TourRepository struct {
	DatabaseConnection *gorm.DB
	Cli                *mongo.Client
	Logger             *log.Logger
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
	err := tour.Validate()
	if err != nil {
		return *tour, err
	}
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Second)
	defer cancel()

	toursCollection := repo.getCollection()

	_, err = toursCollection.InsertOne(ctx, &tour)
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
	update := bson.M{}

	// Iterate over the fields of the tour object and add them to the update map
	val := reflect.ValueOf(tour).Elem()
	typ := val.Type()
	for i := 0; i < val.NumField(); i++ {
		fieldName := typ.Field(i).Name
		fieldValue := val.Field(i).Interface()
		update[fieldName] = fieldValue
	}

	result, err := toursCollection.UpdateOne(ctx, filter, update)
	repo.Logger.Printf("Documents matched: %v\n", result.MatchedCount)
	repo.Logger.Printf("Documents updated: %v\n", result.ModifiedCount)

	if err != nil {
		repo.Logger.Println(err)
		return model.Tour{}, err
	}
	updatedTour, err := repo.GetById(tour.Id)
	if err != nil {
		return model.Tour{}, err
	}

	return updatedTour, nil
}
func (repo *TourRepository) Delete(tourId string) error {

	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Second)
	defer cancel()
	toursCollection := repo.getCollection()

	objID, _ := primitive.ObjectIDFromHex(tourId)
	filter := bson.D{{Key: "_id", Value: objID}}
	result, err := toursCollection.DeleteOne(ctx, filter)
	if err != nil {
		return err
	}
	repo.Logger.Printf("Documents deleted: %v\n", result.DeletedCount)
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
func (repo *TourRepository) GetById(id primitive.ObjectID) (model.Tour, error) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Second)
	defer cancel()

	toursCollection := repo.getCollection()
	filter := bson.M{"_id": id}

	var tour model.Tour
	err := toursCollection.FindOne(ctx, filter).Decode(&tour)
	if err != nil {
		return tour, err
	}

	return tour, nil
}
func (tr *TourRepository) getCollection() *mongo.Collection {
	tourDatabase := tr.Cli.Database("tourService")
	toursCollection := tourDatabase.Collection("tours")
	return toursCollection
}
