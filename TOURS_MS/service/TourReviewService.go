package service

import (
	"fmt"
	"tours/model"
	"tours/repo"
)

type TourReviewService struct {
	TourReviewRepository *repo.TourReviewRepository
}

func (service *TourReviewService) GetAll() ([]model.TourReview, error) {
	reviews, err := service.TourReviewRepository.GetAll()
	if err != nil {
		return nil, fmt.Errorf(fmt.Sprintf("There is some error"))
	}
	return reviews, nil
}

func (service *TourReviewService) Create(tp *model.TourReview) (*model.TourReview, error) {
	review, err := service.TourReviewRepository.Create(tp)
	if err != nil {

		fmt.Println(err.Error())
		return nil, err
	}
	return &review, nil
}
