package service

import (
	"ENCOUNTERS-MS/model"
	"ENCOUNTERS-MS/repo"
	"fmt"
)

type KeypointEncounterService struct {
	KeypointEncounterRepo *repo.KeypointEncounterRepository
}

func (service *KeypointEncounterService) GetPagedByKeypoint(keypointId string) ([]*model.KeypointEncounter, error) {
	result, err := service.KeypointEncounterRepo.GetPagedByKeypoint(keypointId)
	if err != nil {
		return nil, fmt.Errorf("Error fetching paged keypoint encounters: %s", err)
	}
	return result, nil
}

func (service *KeypointEncounterService) Create(keypointEncounter *model.KeypointEncounter) (*model.KeypointEncounter, error) {
	keypointEncounter.Encounter.ApprovalStatus = model.ApprovalSystemApproved
	result, err := service.KeypointEncounterRepo.Create(keypointEncounter)
	if err != nil {
		return nil, fmt.Errorf("Error creating keypoint encounter: %s", err)
	}
	return result, nil
}

func (service *KeypointEncounterService) Delete(keypointEncounterId string) error {
	keypointEncounter, err := service.KeypointEncounterRepo.GetById(keypointEncounterId)
	if err != nil {
		return fmt.Errorf("Error finding keypoint encounter: %s", err)
	}
	if keypointEncounter == nil {
		return fmt.Errorf("Encounter is not found")
	}

	if err := service.KeypointEncounterRepo.Delete(keypointEncounterId); err != nil {
		return fmt.Errorf("Error deleting keypoint encounter: %s", err)
	}

	return nil
}

func (service *KeypointEncounterService) Update(keypointEncounter *model.KeypointEncounter) (*model.KeypointEncounter, error) {
	result, err := service.KeypointEncounterRepo.Update(keypointEncounter)
	if err != nil {
		return nil, fmt.Errorf("Error updating keypoint encounter: %s", err)
	}
	return result, nil
}
