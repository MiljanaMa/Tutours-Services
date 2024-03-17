package service

import (
	"ENCOUNTERS-MS/model"
	"ENCOUNTERS-MS/repo"
	"fmt"
)

type EncounterService struct {
	EncounterRepo *repo.EncounterRepository
}

func (service *EncounterService) GetApproved() ([]*model.Encounter, error) {

	if encounters, err := service.EncounterRepo.GetApproved(); err == nil {
		return encounters, nil
	}

	return nil, fmt.Errorf("Couldn't find any approved")

}
