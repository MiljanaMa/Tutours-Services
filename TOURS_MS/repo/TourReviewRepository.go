package repo

import (
	"gorm.io/gorm"
	"time"
	"tours/model"
)

type TourReviewRepository struct {
	DatabaseConnection *gorm.DB
}

func (repo *TourReviewRepository) GetAll() ([]model.TourReview, error) {
	var reviews []model.TourReview
	dbResult := repo.DatabaseConnection.Find(&reviews)
	if dbResult != nil {
		return reviews, dbResult.Error
	}
	return reviews, nil
}

func (repo *TourReviewRepository) Create(review *model.TourReview) (model.TourReview, error) {
	err := review.Validate()
	if err != nil {
		return *review, err
	}

	review.RatingDate = time.Now()

	dbResult := repo.DatabaseConnection.Create(review)
	if dbResult.Error != nil {
		return *review, dbResult.Error
	}
	return *review, nil
}
