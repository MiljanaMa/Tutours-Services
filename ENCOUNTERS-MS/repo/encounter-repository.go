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

func (repo *EncounterRepository) Create(encounter *model.Encounter) (*model.Encounter, error) {
	res := repo.DatabaseConnection.Create(&encounter)
	return encounter, res.Error
}

func (repo *EncounterRepository) Update(encounter *model.Encounter) error {
	return repo.DatabaseConnection.Save(&encounter).Error
}

func (repo *EncounterRepository) GetApprovedByStatus(status model.EncounterStatus) ([]*model.Encounter, error) {
	var encounters []*model.Encounter
	dbResult := repo.DatabaseConnection.Where("approval_status IN (?) AND status = ?",
		[]string{"SYSTEM_APPROVED", "ADMIN_APPROVED"}, status).Find(&encounters)

	if dbResult.Error != nil {
		return encounters, dbResult.Error
	}

	return encounters, nil
}

func (repo *EncounterRepository) GetApprovedByStatusAndType(status model.EncounterStatus, encounterType model.EncounterType) ([]*model.Encounter, error) {
	var encounters []*model.Encounter
	dbResult := repo.DatabaseConnection.Where("approval_status IN (?) AND status = ? AND type = ?",
		[]string{"SYSTEM_APPROVED", "ADMIN_APPROVED"}, status, encounterType).Find(&encounters)

	if dbResult.Error != nil {
		return encounters, dbResult.Error
	}

	return encounters, nil
}

func (repo *EncounterRepository) GetByUser(userId int) ([]*model.Encounter, error) {
	var encounters []*model.Encounter
	if dbResult := repo.DatabaseConnection.Where("user_id = ?", userId).Find(&encounters); dbResult.Error != nil {
		return encounters, dbResult.Error
	}

	return encounters, nil
}

func (repo *EncounterRepository) GetTouristCreatedEncounters() ([]*model.Encounter, error) {
	var encounters []*model.Encounter
	if dbResult := repo.DatabaseConnection.Where("approval_status != ?", "SYSTEM_APPROVED").Find(&encounters); dbResult != nil {
		return encounters, dbResult.Error
	}

	return encounters, nil

}
func (repo *EncounterRepository) GetAll() ([]*model.Encounter, error) {
	var encounters []*model.Encounter
	dbResult := repo.DatabaseConnection.Find(&encounters)

	if dbResult.Error != nil {
		return encounters, dbResult.Error
	}

	return encounters, nil
}

func (repo *EncounterRepository) GetByType(long float64, lat float64, encounterType model.EncounterType) ([]*model.Encounter, error) {
	var encounters []*model.Encounter
	dbResult := repo.DatabaseConnection.Where("type = ?", encounterType).Find(&encounters)

	if dbResult.Error != nil {
		return encounters, dbResult.Error
	}

	return encounters, nil
}

func (repo *EncounterRepository) Delete(id int) error {
	var encounter model.Encounter
	if dbResult := repo.DatabaseConnection.Where("id = ?", id).Delete(&encounter); dbResult.Error != nil {
		return dbResult.Error
	}
	return nil

}
