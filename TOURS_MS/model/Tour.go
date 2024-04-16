package model

import (
	"encoding/json"
	"errors"
	"go.mongodb.org/mongo-driver/bson/primitive"
	"time"
	"tours/model/enum"
	"tours/model/helper"
)

type Tour struct {
	Id               primitive.ObjectID  `bson:"_id,omitempty" json:"id"`
	UserId           int                 `bson:"user_id,omitempty" json:"user_id"`
	Name             string              `bson:"name,omitempty" json:"name"`
	Description      string              `bson:"description,omitempty" json:"description"`
	Price            float64             `bson:"price,omitempty" json:"price"`
	Duration         int                 `bson:"duration,omitempty" json:"duration"`
	Distance         float64             `bson:"distance,omitempty" json:"distance"`
	Difficulty       enum.TourDifficulty `bson:"difficulty,omitempty" json:"difficulty"`
	TransportType    enum.TransportType  `bson:"transport_type,omitempty" json:"transport_type"`
	Status           enum.TourStatus     `bson:"status,omitempty" json:"status"`
	StatusUpdateTime time.Time           `bson:"status_update_time" json:"status_update_time"`
	Tags             helper.ArrayString  `bson:"tags,omitempty" json:"tags"`
	Keypoints        []Keypoint          //`gorm:"foreignKey:TourId"`
	TourReviews      []TourReview        //`gorm:"foreignKey:TourId"`
}

// use when creating
func (tour *Tour) Validate() error {
	if tour.Name == "" {
		return errors.New("Invalid name")
	}
	if tour.Description == "" {
		return errors.New("Invalid Description")
	}
	if tour.Price < 0 {
		return errors.New("Invalid Price")
	}
	if tour.Duration < 0 {
		return errors.New("Invalid Duration")
	}
	if tour.Distance < 0 {
		return errors.New("Invalid Distance")
	}
	if len(tour.Tags) == 0 {
		return errors.New("Not enough Tags")
	}

	return nil
}

// use when updating
func (tour *Tour) ValidateUpdate(oldTour *Tour) error {
	if tour.Status == enum.ARCHIVED && oldTour.Status != enum.PUBLISHED {
		return errors.New("Tour is not published yet")
	}
	//this stayed like this because collegues left tour update without keypoints
	if len(oldTour.Keypoints) < 2 && tour.Status == enum.PUBLISHED {
		return errors.New("Not enough Key Points")
	}
	return nil
}
func (t *Tour) IsEmpty() bool {
	return t.Name == ""
}

func (t *Tour) MarshalJSON() ([]byte, error) {
	return json.Marshal(struct {
		Id               primitive.ObjectID
		UserId           int
		Name             string
		Description      string
		Price            float64
		Duration         int
		Distance         float64
		Difficulty       string
		TransportType    string
		Status           string
		StatusUpdateTime time.Time
		Tags             helper.ArrayString
		Keypoints        []Keypoint
		TourReviews      []TourReview
	}{
		Id:               t.Id,
		UserId:           t.UserId,
		Name:             t.Name,
		Description:      t.Description,
		Price:            t.Price,
		Duration:         t.Duration,
		Distance:         t.Distance,
		Difficulty:       t.Difficulty.ToString(),
		TransportType:    t.TransportType.ToString(),
		Status:           t.Status.ToString(),
		StatusUpdateTime: t.StatusUpdateTime,
		Tags:             t.Tags,
		Keypoints:        t.Keypoints,
		TourReviews:      t.TourReviews,
	})
}
func (t *Tour) UnmarshalJSON(data []byte) error {

	var temp struct {
		Id               int
		UserId           int
		Name             string
		Description      string
		Price            float64
		Duration         int
		Distance         float64
		Difficulty       string
		TransportType    string
		Status           string
		StatusUpdateTime time.Time
		Tags             helper.ArrayString
		Keypoints        []Keypoint
		TourReviews      []TourReview
	}

	if err := json.Unmarshal(data, &temp); err != nil {
		return err
	}

	t.Id = primitive.ObjectID{}
	t.UserId = temp.UserId
	t.Name = temp.Name
	t.Description = temp.Description
	t.Price = temp.Price
	t.Duration = temp.Duration
	t.Distance = temp.Distance
	t.Difficulty = enum.FromStringToDifficulty(temp.Difficulty)
	t.TransportType = enum.FromStringToType(temp.TransportType)
	t.Status = enum.FromStringToStatus(temp.Status)
	t.StatusUpdateTime = temp.StatusUpdateTime
	t.Tags = temp.Tags
	t.Keypoints = temp.Keypoints
	t.TourReviews = temp.TourReviews

	return nil
}
