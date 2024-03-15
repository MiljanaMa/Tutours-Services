package service

import (
	"tours/model"
	"tours/repo"
)

type TourService struct {
	TourRepository *repo.TourRepository
}

/*
	func (service *TourService) GetById(id string) (*model.Tour, error) {
		tour, err := service.TourRepository.GetById(id)
		if err != nil {
			return nil, fmt.Errorf(fmt.Sprintf("menu item with id %s not found", id))
		}
		return &tour, nil
	}
*/
func (service *TourService) Create(tour *model.Tour) error {
	err := service.TourRepository.Create(tour)
	if err != nil {
		return err
	}
	return nil
}
func (service *TourService) GetAll() ([]model.Tour, error) {
	var tours []model.Tour
	tours, err := service.TourRepository.GetAll()
	if err != nil {
		return nil, err
	}

	return tours, nil
}

func (service *TourService) GetAllByAuthorId(userId int) ([]model.Tour, error) {
	var tours []model.Tour
	tours, err := service.TourRepository.GetAllByAuthor(userId)
	if err != nil {
		return nil, err
	}
	return tours, nil
}

func (service *TourService) GetById(id int) (model.Tour, error) {
	var tour model.Tour
	tour, err := service.TourRepository.GetById(id)
	if err != nil {
		return tour, err
	}
	return tour, nil
}
