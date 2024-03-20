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
