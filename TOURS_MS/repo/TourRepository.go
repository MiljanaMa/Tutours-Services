package repo

import (
	"tours/model"

	"gorm.io/gorm"
)

type TourRepository struct {
	DatabaseConnection *gorm.DB
}

func (repo *TourRepository) GetById(id string) (model.Tour, error) {
	tour := model.Tour{}
	dbResult := repo.DatabaseConnection.First(&tour, "id = ?", id)
	if dbResult != nil {
		return tour, dbResult.Error
	}
	return tour, nil
}

func (repo *TourRepository) Create(tour *model.Tour) error {
	dbResult := repo.DatabaseConnection.Create(tour)
	if dbResult.Error != nil {
		return dbResult.Error
	}
	return nil
}
