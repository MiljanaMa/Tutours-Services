package repo

import (
	"ENCOUNTERS-MS/model"

	"gorm.io/gorm"
)

type EncounterRepository struct {
	DatabaseConnection *gorm.DB
}

func (repo *EncounterRepository) GetApproved() ([]*model.Encounter, error) {
	var encounters []*model.Encounter
	dbResult := repo.DatabaseConnection.Where("approval_status IN (?)",
		[]string{"SYSTEM_APPROVED", "ADMIN_APPROVED"}).Find(&encounters)

	if dbResult.Error != nil {
		return encounters, dbResult.Error
	}

	return encounters, nil
}
