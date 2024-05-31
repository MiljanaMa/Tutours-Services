package repo

import (
	"ENCOUNTERS-MS/model"
	"gorm.io/gorm"
)

type EncounterCompletionRepository struct {
	DatabaseConnection *gorm.DB
}

func (repo *EncounterCompletionRepository) GetPagedByUser(userId string) ([]*model.EncounterCompletion, error) {
	//NOTE: change user id type later to uuid or something
	var encounterCompletions []*model.EncounterCompletion
	dbResult := repo.DatabaseConnection.Where("user_id = ?", userId).Preload("Encounter").Find(&encounterCompletions)

	if dbResult.Error != nil {
		return encounterCompletions, dbResult.Error
	}

	return encounterCompletions, nil
}

func (repo *EncounterCompletionRepository) GetByUserAndEncounter(userId, encounterId int) (*model.EncounterCompletion, error) {
	var foundEnounterCompletion *model.EncounterCompletion
	dbResult := repo.DatabaseConnection.Where("user_id = ? AND encounter_id = ?", userId, encounterId).
		Preload("Encounter").
		First(&foundEnounterCompletion)

	if dbResult.Error != nil {
		return nil, dbResult.Error
	}
	return foundEnounterCompletion, nil
}

func (repo *EncounterCompletionRepository) HasUserStartedEncounter(userId, encounterId int) bool {
	var foundEnounterCompletion *model.EncounterCompletion
	dbResult := repo.DatabaseConnection.Where("user_id = ? AND encounter_id = ? AND status = ?", userId, encounterId, "STARTED").
		Preload("Encounter").
		First(&foundEnounterCompletion)

	if dbResult.Error != nil {
		return false
	}
	return true
}

func (repo *EncounterCompletionRepository) GetById(id int) (*model.EncounterCompletion, error) {
	var encounter model.EncounterCompletion
	if err := repo.DatabaseConnection.First(&encounter, id).Error; err != nil {
		return nil, err
	}
	return &encounter, nil
}

func (repo *EncounterCompletionRepository) Update(encounterCompletion *model.EncounterCompletion) error {
	dbResult := repo.DatabaseConnection.Save(encounterCompletion)
	return dbResult.Error
}

func (repo *EncounterCompletionRepository) Create(encounterCompletion *model.EncounterCompletion) (*model.EncounterCompletion, error) {
	res := repo.DatabaseConnection.Create(&encounterCompletion)
	return encounterCompletion, res.Error
}
