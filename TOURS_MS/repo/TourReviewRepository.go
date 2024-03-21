package repo

import (
	"errors"
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

	var existingReview model.TourReview
	dbResult1 := repo.DatabaseConnection.Where("tour_id = ? AND user_id = ?", review.TourId, review.UserId).Find(&existingReview)
	if dbResult1.RowsAffected > 0 {
		return existingReview, errors.New("Review already exists")
	} else if dbResult1.Error != nil && !errors.Is(dbResult1.Error, gorm.ErrRecordNotFound) {
		return *review, dbResult1.Error
	}

	review.RatingDate = time.Now()

	dbResult := repo.DatabaseConnection.Create(review)
	if dbResult.Error != nil {
		return *review, dbResult.Error
	}
	return *review, nil
}
