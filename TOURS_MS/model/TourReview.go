package model

import (
	"errors"
	"time"
	"tours/model/helper"
)

type TourReview struct {
	Id      int `gorm:"primaryKey"`
	Rating  int
	Comment string
	UserId  int
	TourId  int // `gorm:"foreignKey:TourId"`
	//Tour       *Tour
	VisitDate  time.Time
	RatingDate time.Time
	ImageLinks helper.ArrayString
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
