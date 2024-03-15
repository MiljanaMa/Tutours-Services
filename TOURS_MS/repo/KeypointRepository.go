package repo

import (
	"tours/model"

	"gorm.io/gorm"
)

type KeypointRepository struct {
	DatabaseConnection *gorm.DB
}

func (repo *KeypointRepository) GetById(id string) (model.Keypoint, error) {
	keypoint := model.Keypoint{}
	dbResult := repo.DatabaseConnection.First(&keypoint, "id = ?", id)
	if dbResult != nil {
		return keypoint, dbResult.Error
	}
	return keypoint, nil
}

func (repo *KeypointRepository) GetByTourId(tourId string) ([]model.Keypoint, error) {
	keypoints := []model.Keypoint{}
	dbResult := repo.DatabaseConnection.Where("tour_id = ?", tourId).Find(&keypoints)
	if dbResult != nil {
		return keypoints, dbResult.Error
	}
	return keypoints, nil
}

func (repo *KeypointRepository) Create(keypoint *model.Keypoint) (model.Keypoint, error) {
	dbResult := repo.DatabaseConnection.Create(keypoint)
	if dbResult.Error != nil {
		return *keypoint, dbResult.Error
	}
	return *keypoint, nil
}
