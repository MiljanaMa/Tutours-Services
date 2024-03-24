package service

import (
	"ENCOUNTERS-MS/model"
	"ENCOUNTERS-MS/repo"
	"fmt"
)

type EncounterCompletionService struct {
	EncounterCompletionRepo *repo.EncounterCompletionRepository
}

func (service *EncounterCompletionService) GetPagedByUser(userId string) ([]*model.EncounterCompletion, error) {
	if encounterCompletions, err := service.EncounterCompletionRepo.GetPagedByUser(userId); err == nil {
		return encounterCompletions, nil
	}
	return nil, fmt.Errorf("Error finding encounter completions")
}

func (service *EncounterCompletionService) FinishEncounter(userId string, encounterId string) (*model.EncounterCompletion, error) {
	encounterCompletion, err := service.EncounterCompletionRepo.GetByUserAndEncounter(userId, encounterId)
	if err != nil {
		return encounterCompletion, err
	}
	if encounterCompletion == nil {
		return nil, fmt.Errorf("Encounter completion not found for user and encounter")
	}

	encounterCompletion.Status = model.CompletionStatusCompleted
	if err := service.EncounterCompletionRepo.Update(encounterCompletion); err != nil {
		return encounterCompletion, err
	}

	//NOTE: XP for user, not needed now

	return encounterCompletion, nil
}
