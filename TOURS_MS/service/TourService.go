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
func (service *TourService) Create(tour *model.Tour) (model.Tour, error) {
	savedTour, err := service.TourRepository.Create(tour)
	if err != nil {
		return savedTour, err
	}
	return savedTour, nil
}
func (service *TourService) Update(tour *model.Tour) (model.Tour, error) {
	updatedTour, err := service.TourRepository.Update(tour)
	if err != nil {
		return updatedTour, err
	}
	return updatedTour, nil
}
func (service *TourService) Delete(tourId int) error {
	err := service.TourRepository.Delete(tourId)
	if err != nil {
		return err
	}
	return nil
}
func (service *TourService) GetAll(limit, page int) ([]model.Tour, error) {
	var tours []model.Tour
	tours, err := service.TourRepository.GetAll(limit, page)
	if err != nil {
		return nil, err
	}

	return tours, nil
}

func (service *TourService) GetAllByAuthorId(limit, page, userId int) ([]model.Tour, error) {
	var tours []model.Tour
	tours, err := service.TourRepository.GetAllByAuthor(limit, page, userId)
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
