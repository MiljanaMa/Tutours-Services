package model

import (
	"encoding/json"
	"errors"
	"time"
	"tours/model/helper"
)

type TourReview struct {
	Id         int                `bson:"_id,omitempty" json:"id"`
	Rating     int                `bson:"rating,omitempty" json:"rating"`
	Comment    string             `bson:"comment,omitempty" json:"comment"`
	UserId     int                `bson:"user_id,omitempty" json:"user_id"`
	TourId     int                `bson:"tour_id,omitempty" json:"tour_id"`
	VisitDate  time.Time          `bson:"visit_date,omitempty" json:"visit_date"`
	RatingDate time.Time          `bson:"rating_date,omitempty" json:"rating_date"`
	ImageLinks helper.ArrayString `bson:"image_links,omitempty" json:"image_links"`
}

func (review *TourReview) Validate() error {
	if review.Comment == "" {
		return errors.New("Comment must be left")
	}
	if review.Rating < 1 || review.Rating > 5 {
		return errors.New("Invalid Rating")
	}
	if review.VisitDate.After(time.Now()) {
		return errors.New("Invalid Visiting Date")
	}
	if len(review.ImageLinks) == 0 {
		return errors.New("Must leave some pictures")
	}

	return nil
}
func (t *TourReview) UnmarshalJSON(data []byte) error {

	var temp struct {
		Id         int
		Rating     int
		Comment    string
		UserId     int
		TourId     int
		VisitDate  time.Time
		RatingDate time.Time
		ImageLinks helper.ArrayString
	}

	if err := json.Unmarshal(data, &temp); err != nil {
		return err
	}

	t.Id = temp.Id
	t.UserId = temp.UserId
	t.Rating = temp.Rating
	t.Comment = temp.Comment
	t.TourId = temp.TourId
	t.VisitDate = temp.VisitDate
	t.RatingDate = temp.RatingDate
	t.ImageLinks = temp.ImageLinks

	return nil
}
