package repo

import (
	"context"
	"fmt"
	"go.mongodb.org/mongo-driver/bson"
	"go.mongodb.org/mongo-driver/mongo"
	"log"
	"time"
	"tours/model"
)

type TourReviewRepository struct {
	Cli    *mongo.Client
	Logger *log.Logger
}

func (repo *TourReviewRepository) GetAll() ([]model.TourReview, error) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Second)
	defer cancel()

	reviewCollection := repo.getCollection()

	var reviews []model.TourReview
	reviewsCursor, err := reviewCollection.Find(ctx, bson.M{})
	if err != nil {
		return nil, err
	}
	if err := reviewsCursor.All(ctx, &reviews); err != nil {
		return nil, err
	}

	return reviews, nil
}

func (repo *TourReviewRepository) Create(review *model.TourReview) (model.TourReview, error) {
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Second)
	defer cancel()

	reviewCollection := repo.getCollection()
	counterCollection := repo.getCounter()

	filterExist := bson.M{"tour_id": review.TourId, "user_id": review.UserId}
	result := reviewCollection.FindOne(ctx, filterExist)
	if result.Err() != nil {
		if result.Err() != mongo.ErrNoDocuments {
			return *review, result.Err()
		}
	} else {
		return *review, fmt.Errorf("review already exists")
	}

	err := review.Validate()
	if err != nil {
		return *review, err
	}
	review.RatingDate = time.Now()

	filter := bson.M{"name": "review"}
	var counter struct {
		Id    int    `bson:"_id"`
		Value int    `bson:"value"`
		Name  string `bson:"name"`
	}
	errCounter := counterCollection.FindOne(ctx, filter).Decode(&counter)
	if errCounter != nil {
		return *review, errCounter
	}

	currentCounterValue := counter.Value

	review.Id = currentCounterValue
	_, err = reviewCollection.InsertOne(ctx, &review)
	if err != nil {
		return *review, err
	}

	newCounterValue := currentCounterValue + 1

	update := bson.M{"$set": bson.M{"value": newCounterValue}}
	_, err = counterCollection.UpdateOne(ctx, filter, update)
	if err != nil {
		return *review, err
	}
	return *review, nil
}
func (tr *TourReviewRepository) getCollection() *mongo.Collection {
	reviewDatabase := tr.Cli.Database("tourService")
	reviewCollection := reviewDatabase.Collection("reviews")
	return reviewCollection
}
func (tr *TourReviewRepository) getCounter() *mongo.Collection {
	tourDatabase := tr.Cli.Database("tourService")
	counter := tourDatabase.Collection("counter")
	return counter
}
