package repo

import (
	"ENCOUNTERS-MS/model"
	"context"

	"go.opentelemetry.io/otel"
	"go.opentelemetry.io/otel/attribute"
	"gorm.io/gorm"
)

type EncounterRepository struct {
	DatabaseConnection *gorm.DB
}

func (repo *EncounterRepository) GetApproved(ctx context.Context) ([]*model.Encounter, error) {
	tracer := otel.Tracer("encounter-repo")
	ctx, span := tracer.Start(ctx, "GetApproved Repo")
	defer span.End()

	var encounters []*model.Encounter
	dbResult := repo.DatabaseConnection.Where("approval_status IN (?)",
		[]string{"SYSTEM_APPROVED", "ADMIN_APPROVED"}).Find(&encounters)

	if dbResult.Error != nil {
		return encounters, dbResult.Error
	}
	span.SetAttributes(attribute.Int("number_of_encounters", len(encounters)))
	return encounters, nil
}
func (repo *EncounterRepository) GetById(id int) (*model.Encounter, error) {
	var encounter model.Encounter
	if err := repo.DatabaseConnection.First(&encounter, id).Error; err != nil {
		return nil, err
	}
	return &encounter, nil
}

func (repo *EncounterRepository) Create(encounter *model.Encounter) (*model.Encounter, error) {
	res := repo.DatabaseConnection.Create(&encounter)
	return encounter, res.Error
}

func (repo *EncounterRepository) Update(encounter *model.Encounter) error {
	res := repo.DatabaseConnection.Save(&encounter)
	return res.Error
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
	var completions []model.EncounterCompletion
	if err := repo.DatabaseConnection.Where("encounter_id = ?", id).Find(&completions).Error; err != nil {
		return err
	}

	// Delete related rows from the "encounter_completions" table
	for _, completion := range completions {
		if err := repo.DatabaseConnection.Delete(&completion).Error; err != nil {
			return err
		}
	}
	var keypoints []model.KeypointEncounter
	if err := repo.DatabaseConnection.Where("encounter_id = ?", id).Find(&keypoints).Error; err != nil {
		return err
	}

	// Delete related rows from the "encounter_completions" table
	for _, keypointe := range keypoints {
		if err := repo.DatabaseConnection.Delete(&keypointe).Error; err != nil {
			return err
		}
	}
	if dbResult := repo.DatabaseConnection.Where("id = ?", id).Select("TourReviews").Delete(&encounter); dbResult.Error != nil {
		return dbResult.Error
	}
	return nil

}
