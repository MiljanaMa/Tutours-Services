package repo

import (
	"gorm.io/gorm"
	"tours/model"
)

type TourRepository struct {
	DatabaseConnection *gorm.DB
}

/*
func (repo *TourRepository) GetById(id string) (model.Tour, error) {
	tour := model.Tour{}
	dbResult := repo.DatabaseConnection.First(&tour, "id = ?", id)
	if dbResult != nil {
		return tour, dbResult.Error
	}
	return tour, nil
}*/

func (repo *TourRepository) Create(tour *model.Tour) (model.Tour, error) {
	err := tour.Validate()
	if err != nil {
		return *tour, err
	}
	dbResult := repo.DatabaseConnection.Create(tour)
	if dbResult.Error != nil {
		return *tour, dbResult.Error
	}
	return *tour, nil
}
func (repo *TourRepository) Update(tour *model.Tour) (model.Tour, error) {
	tourForUpdate, err := repo.GetById(tour.Id)
	if err != nil {
		return *tour, err
	}

	//validation for archiving and publishing
	err = tour.Validate()
	if err != nil {
		return *tour, err
	}

	err = tour.ValidateUpdate(&tourForUpdate)
	if err != nil {
		return *tour, err
	}

	dbResult := repo.DatabaseConnection.Save(tour)
	if dbResult.Error != nil {
		return *tour, dbResult.Error
	}
	return *tour, nil
}
func (repo *TourRepository) Delete(tourId int) error {

	tour, err := repo.GetById(tourId)
	if err != nil {
		return err
	}
	err = repo.DatabaseConnection.Select("Keypoints", "TourReviews").Delete(tour).Error
	if err != nil {
		return err
	}
	return nil
}

func (repo *TourRepository) GetAllByAuthor(limit, page, userId int) ([]model.Tour, error) {
	var tours []model.Tour
	dbResult := repo.DatabaseConnection.Where("user_id = ?", userId).Preload("Keypoints").Find(&tours)
	if dbResult.Error != nil {
		return nil, dbResult.Error
	}
	return tours, nil
}
func (repo *TourRepository) GetAll(limit, page int) ([]model.Tour, error) {
	var tours []model.Tour
	dbResult := repo.DatabaseConnection.Preload("Keypoints").Find(&tours)
	if dbResult.Error != nil {
		return nil, dbResult.Error
	}

	return tours, nil
}
func (repo *TourRepository) GetById(id int) (model.Tour, error) {
	var tour model.Tour
	dbResult := repo.DatabaseConnection.Where("id = ?", id).Preload("Keypoints").Find(&tour)
	if dbResult.Error != nil {
		return tour, dbResult.Error
	}
	return tour, nil
}
