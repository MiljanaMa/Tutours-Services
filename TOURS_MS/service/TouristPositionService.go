package service

import (
	"fmt"
	"tours/model"
	"tours/repo"
)

type TouristPositionService struct {
	TouristPositionRepository *repo.TouristPositionRepository
}

func (service *TouristPositionService) GetById(id int) (model.TouristPosition, error) {
	var position model.TouristPosition
	position, err := service.TouristPositionRepository.GetById(id)
	if err != nil {
		return position, err
	}
	return position, nil
}

func (service *TouristPositionService) GetByUserId(userId int) (model.TouristPosition, error) {
	var position model.TouristPosition
	position, err := service.TouristPositionRepository.GetByUserId(userId)
	if err != nil {
		return position, err
	}
	return position, nil
}

func (service *TouristPositionService) Create(tp *model.TouristPosition) (*model.TouristPosition, error) {
	position, err := service.TouristPositionRepository.Create(tp)
	if err != nil {
		return nil, fmt.Errorf(err.Error())
	}
	return &position, nil
}
func (service *TouristPositionService) Update(tp *model.TouristPosition) (model.TouristPosition, error) {
	updatedPosition, err := service.TouristPositionRepository.Update(tp)
	if err != nil {
		return updatedPosition, err
	}
	return updatedPosition, nil
}
