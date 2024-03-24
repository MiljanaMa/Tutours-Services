package repo

import (
	"ENCOUNTERS-MS/model"
	"fmt"

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

func (repo *EncounterCompletionRepository) GetByUserAndEncounter(userId string, encounterId string) (*model.EncounterCompletion, error) {
	var foundEnounterCompletion *model.EncounterCompletion
	dbResult := repo.DatabaseConnection.Where(" encounter_id = ? ", encounterId).Preload("Encounter").Find(&foundEnounterCompletion)

	if dbResult.Error != nil {
		return foundEnounterCompletion, dbResult.Error
	}

	fmt.Println(string(foundEnounterCompletion.Status))
	return foundEnounterCompletion, nil
}

func (repo *EncounterCompletionRepository) Update(encounterCompletion *model.EncounterCompletion) error {
	dbResult := repo.DatabaseConnection.Save(encounterCompletion)
	return dbResult.Error
}
