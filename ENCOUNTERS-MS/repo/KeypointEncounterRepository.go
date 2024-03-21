package repo

import (
	"ENCOUNTERS-MS/model"
	"fmt"

	"gorm.io/gorm"
)

type KeypointEncounterRepository struct {
	DatabaseConnection *gorm.DB
}

func (repo *KeypointEncounterRepository) GetPagedByKeypoint(keypointId string) ([]*model.KeypointEncounter, error) {
	var keypointEncounters []*model.KeypointEncounter
	result := repo.DatabaseConnection.Where("key_point_id = ?", keypointId).Preload("Encounter").Find(&keypointEncounters)
	if result.Error != nil {
		return nil, fmt.Errorf("error fetching paged keypoint encounters: %s", result.Error)
	}
	return keypointEncounters, nil
}

func (repo *KeypointEncounterRepository) GetById(id string) (*model.KeypointEncounter, error) {
	var keypointEncounter model.KeypointEncounter
	result := repo.DatabaseConnection.Where("id = ?", id).First(&keypointEncounter)
	if result.Error != nil {
		return nil, fmt.Errorf("failed to get keypoint encounter: %w", result.Error)
	}
	return &keypointEncounter, nil
}

func (repo *KeypointEncounterRepository) Create(keypointEncounter *model.KeypointEncounter) (*model.KeypointEncounter, error) {
	result := repo.DatabaseConnection.Create(keypointEncounter) //add validation sometime
	if result.Error != nil {
		return nil, fmt.Errorf("error creating keypoint encounter: %s", result.Error)
	}
	return keypointEncounter, nil
}

func (repo *KeypointEncounterRepository) Delete(keypointEncounterId string) error {
	result := repo.DatabaseConnection.Delete(&model.KeypointEncounter{}, keypointEncounterId)
	if result.Error != nil {
		return fmt.Errorf("error deleting keypoint encounter: %s", result.Error)
	}
	return nil
}

func (repo *KeypointEncounterRepository) Update(keypointEncounter *model.KeypointEncounter) (*model.KeypointEncounter, error) {
	result := repo.DatabaseConnection.Save(keypointEncounter) //validation here too
	if result.Error != nil {
		return nil, fmt.Errorf("error updating keypoint encounter: %s", result.Error)
	}
	return keypointEncounter, nil
}
