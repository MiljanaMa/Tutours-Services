package repo

import (
	"ENCOUNTERS-MS/model"

	"gorm.io/gorm"
)

type EncounterCompletionRepository struct {
	DatabaseConnection *gorm.DB
}

func (repo *EncounterCompletionRepository) GetPagedByUser(userId string) ([]*model.EncounterCompletion, error) {
	//TODO: change user id type later to uuid or something
	var encounterCompletions []*model.EncounterCompletion
	dbResult := repo.DatabaseConnection.Where("user_id = ?", userId).Preload("Encounter").Find(&encounterCompletions)

	if dbResult.Error != nil {
		return encounterCompletions, dbResult.Error
	}

	return encounterCompletions, nil
}
