package repo

import (
	"tours/model"

	"gorm.io/gorm"
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

func (repo *TourRepository) Create(tour *model.Tour) error {
	dbResult := repo.DatabaseConnection.Create(tour)
	if dbResult.Error != nil {
		return dbResult.Error
	}
	return nil
}
func (repo *TourRepository) GetAllByAuthor(userId int) ([]model.Tour, error) {
	var tours []model.Tour
	dbResult := repo.DatabaseConnection.Where("user_id = ?", userId). /*.Preload("KeyPoints")*/ Find(&tours)
	if dbResult.Error != nil {
		return nil, dbResult.Error
	}
	return tours, nil
}
func (repo *TourRepository) GetAll() ([]model.Tour, error) {
	var tours []model.Tour
	dbResult := repo.DatabaseConnection. /*.Preload("KeyPoints")*/ Find(&tours)
	if dbResult.Error != nil {
		return nil, dbResult.Error
	}

	return tours, nil
}
func (repo *TourRepository) GetById(id int) (model.Tour, error) {
	var tour model.Tour
	dbResult := repo.DatabaseConnection.Where("id = ?", id). /*.Preload("KeyPoints")*/ Find(&tour)
	if dbResult.Error != nil {
		return tour, dbResult.Error
	}
	return tour, nil
}
