package service

import (
	"fmt"
	"tours/model"
	"tours/repo"
)

type TouristPositionService struct {
	TouristPositionRepository *repo.TouristPositionRepository
}

func (service *TouristPositionService) GetById(id string) (*model.TouristPosition, error) {
	position, err := service.TouristPositionRepository.GetById(id)
	if err != nil {
		return nil, fmt.Errorf(fmt.Sprintf("tourist position with id %s not found", id))
	}
	return &position, nil
}

func (service *TouristPositionService) GetByUserId(userId string) (*model.TouristPosition, error) {
	position, err := service.TouristPositionRepository.GetByUserId(userId)
	if err != nil {
		return nil, fmt.Errorf(fmt.Sprintf("tourist position with user id %s not found", userId))
	}
	return &position, nil
}

func (service *TouristPositionService) Create(tp *model.TouristPosition) (*model.TouristPosition, error) {
	position, err := service.TouristPositionRepository.Create(tp)
	if err != nil {
		return nil, fmt.Errorf("error while creating tourist position")
	}
	return &position, nil
}

func (service *TouristPositionService) Update(kp *model.TouristPosition) (*model.TouristPosition, error) {
	position, err := service.TouristPositionRepository.Update(kp)
	if err != nil {
		return nil, fmt.Errorf("error while updating tourist position")
	}
	return &position, nil
}
