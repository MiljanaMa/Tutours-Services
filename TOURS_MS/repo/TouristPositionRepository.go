package repo

import (
	"tours/model"

	"gorm.io/gorm"
)

type TouristPositionRepository struct {
	DatabaseConnection *gorm.DB
}

func (repo *TouristPositionRepository) GetById(id string) (model.TouristPosition, error) {
	position := model.TouristPosition{}
	dbResult := repo.DatabaseConnection.First(&position, "id = ?", id)
	if dbResult != nil {
		return position, dbResult.Error
	}
	return position, nil
}

func (repo *TouristPositionRepository) GetByUserId(userId string) (model.TouristPosition, error) {
	position := model.TouristPosition{}
	dbResult := repo.DatabaseConnection.First(&position, "user_id = ?", userId)
	if dbResult != nil {
		return position, dbResult.Error
	}
	return position, nil
}

func (repo *TouristPositionRepository) Create(position *model.TouristPosition) (model.TouristPosition, error) {
	dbResult := repo.DatabaseConnection.Create(position)
	if dbResult.Error != nil {
		return *position, dbResult.Error
	}
	return *position, nil
}

func (repo *TouristPositionRepository) Update(position *model.TouristPosition) (model.TouristPosition, error) {
	dbResult := repo.DatabaseConnection.Save(position)
	if dbResult.Error != nil {
		return *position, dbResult.Error
	}
	return *position, nil
}
