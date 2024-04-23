package repo

import (
	"context"
	"log"
	"time"
	"tours/model"

	"go.mongodb.org/mongo-driver/bson"
	"go.mongodb.org/mongo-driver/mongo"
)

type KeypointRepository struct {
	Cli    *mongo.Client
	Logger *log.Logger
}

func (repo *KeypointRepository) GetById(id int) (model.Keypoint, error) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Second)
	defer cancel()

	keypointsCollection := repo.getCollection()
	filter := bson.M{"_id": id}

	var keypoint model.Keypoint
	err := keypointsCollection.FindOne(ctx, filter).Decode(&keypoint)
	if err != nil {
		return keypoint, err
	}

	return keypoint, nil
}

func (repo *KeypointRepository) GetByTourId(tourId int) ([]model.Keypoint, error) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Second)
	defer cancel()

	keypointsCollection := repo.getCollection()
	filter := bson.M{"tour_id": tourId}

	var keypoints []model.Keypoint
	keypointsCursor, err := keypointsCollection.Find(ctx, filter)
	if err != nil {
		return nil, err
	}
	defer keypointsCursor.Close(ctx)
	if err := keypointsCursor.All(ctx, &keypoints); err != nil {
		return nil, err
	}

	return keypoints, nil
}

func (repo *KeypointRepository) Create(keypoint *model.Keypoint) (model.Keypoint, error) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Second)
	defer cancel()

	keypointsCollection := repo.getCollection()
	counterCollection := repo.getCounter()

	filter := bson.M{"name": "keypoint"}
	var counter struct {
		Id    int    `bson:"_id"`
		Value int    `bson:"value"`
		Name  string `bson:"name"`
	}
	errCounter := counterCollection.FindOne(ctx, filter).Decode(&counter)
	if errCounter != nil {
		return *keypoint, errCounter
	}

	currentCounterValue := counter.Value

	keypoint.Id = currentCounterValue
	_, err := keypointsCollection.InsertOne(ctx, &keypoint)
	if err != nil {
		return *keypoint, err
	}

	newCounterValue := currentCounterValue + 1

	update := bson.M{"$set": bson.M{"value": newCounterValue}}
	_, err = counterCollection.UpdateOne(ctx, filter, update)
	if err != nil {
		return *keypoint, err
	}
	return *keypoint, nil
}

func (repo *KeypointRepository) Delete(id int) error {

	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Second)
	defer cancel()
	keypointsCollection := repo.getCollection()

	filter := bson.D{{Key: "_id", Value: id}}
	result, err := keypointsCollection.DeleteOne(ctx, filter)
	if err != nil {
		return err
	}
	repo.Logger.Printf("Documents deleted: %v\n", result.DeletedCount)
	return nil
}

func (repo *KeypointRepository) Update(keypoint *model.Keypoint) (model.Keypoint, error) {
	_, err := repo.GetById(keypoint.Id)
	if err != nil {
		return *keypoint, err
	}

	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Second)
	defer cancel()

	keypointsCollection := repo.getCollection()

	filter := bson.M{"_id": keypoint.Id}
	// Create the update map
	update := bson.M{"$set": bson.M{
		"name":        keypoint.Name,
		"latitude":    keypoint.Latitude,
		"longitude":   keypoint.Longitude,
		"description": keypoint.Description,
		"position":    keypoint.Position,
		"image":       keypoint.Image,
		"secret":      keypoint.Secret,
	}}

	result, err := keypointsCollection.UpdateOne(ctx, filter, update)
	repo.Logger.Printf("Documents matched: %v\n", result.MatchedCount)
	repo.Logger.Printf("Documents updated: %v\n", result.ModifiedCount)

	if err != nil {
		repo.Logger.Println(err)
		return model.Keypoint{}, err
	}
	updatedKeypoint, err := repo.GetById(keypoint.Id)
	if err != nil {
		return model.Keypoint{}, err
	}

	return updatedKeypoint, nil
}

func (kr *KeypointRepository) getCollection() *mongo.Collection {
	tourDatabase := kr.Cli.Database("tourService")
	keypointsCollection := tourDatabase.Collection("keypoints")
	return keypointsCollection
}
func (kr *KeypointRepository) getCounter() *mongo.Collection {
	tourDatabase := kr.Cli.Database("tourService")
	counter := tourDatabase.Collection("counter")
	return counter
}
