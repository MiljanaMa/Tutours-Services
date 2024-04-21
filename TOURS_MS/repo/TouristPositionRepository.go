package repo

import (
	"context"
	"log"
	"time"
	"tours/model"

	"go.mongodb.org/mongo-driver/bson"
	"go.mongodb.org/mongo-driver/mongo"
)

type TouristPositionRepository struct {
	Cli    *mongo.Client
	Logger *log.Logger
}

func (repo *TouristPositionRepository) GetById(id int) (model.TouristPosition, error) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Second)
	defer cancel()

	positionsCollection := repo.getCollection()
	filter := bson.M{"_id": id}

	var position model.TouristPosition
	err := positionsCollection.FindOne(ctx, filter).Decode(&position)
	if err != nil {
		return position, err
	}

	return position, nil
}

func (repo *TouristPositionRepository) GetByUserId(userId int) (model.TouristPosition, error) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Second)
	defer cancel()

	positionsCollection := repo.getCollection()
	filter := bson.M{"user_id": userId}

	var position model.TouristPosition
	err := positionsCollection.FindOne(ctx, filter).Decode(&position)
	if err != nil {
		return position, err
	}

	return position, nil
}

func (repo *TouristPositionRepository) Create(position *model.TouristPosition) (model.TouristPosition, error) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Second)
	defer cancel()

	positionsCollection := repo.getCollection()
	counterCollection := repo.getCounter()

	filter := bson.M{"name": "position"}
	var counter struct {
		Id    int    `bson:"_id"`
		Value int    `bson:"value"`
		Name  string `bson:"name"`
	}
	errCounter := counterCollection.FindOne(ctx, filter).Decode(&counter)
	if errCounter != nil {
		return *position, errCounter
	}

	currentCounterValue := counter.Value

	position.Id = currentCounterValue
	_, err := positionsCollection.InsertOne(ctx, &position)
	if err != nil {
		return *position, err
	}

	newCounterValue := currentCounterValue + 1

	update := bson.M{"$set": bson.M{"value": newCounterValue}}
	_, err = counterCollection.UpdateOne(ctx, filter, update)
	if err != nil {
		return *position, err
	}
	return *position, nil
}

func (repo *TouristPositionRepository) Update(positon *model.TouristPosition) (model.TouristPosition, error) {
	_, err := repo.GetById(positon.Id)
	if err != nil {
		return *positon, err
	}

	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Second)
	defer cancel()

	positionsCollection := repo.getCollection()

	filter := bson.M{"_id": positon.Id}
	// Create the update map
	update := bson.M{"$set": bson.M{
		"latitude":   positon.Latitude,
		"longitude":  positon.Longitude,
		"updated_at": positon.UpdatedAt,
	}}

	result, err := positionsCollection.UpdateOne(ctx, filter, update)
	repo.Logger.Printf("Documents matched: %v\n", result.MatchedCount)
	repo.Logger.Printf("Documents updated: %v\n", result.ModifiedCount)

	if err != nil {
		repo.Logger.Println(err)
		return model.TouristPosition{}, err
	}
	updatedPosition, err := repo.GetById(positon.Id)
	if err != nil {
		return model.TouristPosition{}, err
	}

	return updatedPosition, nil
}

func (tr *TouristPositionRepository) getCollection() *mongo.Collection {
	tourDatabase := tr.Cli.Database("tourService")
	positionsCollection := tourDatabase.Collection("positions")
	return positionsCollection
}

func (kr *TouristPositionRepository) getCounter() *mongo.Collection {
	tourDatabase := kr.Cli.Database("tourService")
	counter := tourDatabase.Collection("counter")
	return counter
}
