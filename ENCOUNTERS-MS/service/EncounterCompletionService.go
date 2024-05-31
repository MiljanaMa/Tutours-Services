package service

import (
	"ENCOUNTERS-MS/model"
	"ENCOUNTERS-MS/repo"
	"fmt"
	"time"
)

type EncounterCompletionService struct {
	EncounterCompletionRepo *repo.EncounterCompletionRepository
	EncounterService        *EncounterService
	orchestrator            *FinishEncounterOrchestrator
}

func NewEncounterCompletionService(repo *repo.EncounterCompletionRepository,
	encounterService *EncounterService, orchestrator *FinishEncounterOrchestrator) *EncounterCompletionService {
	return &EncounterCompletionService{
		EncounterCompletionRepo: repo,
		EncounterService:        encounterService,
		orchestrator:            orchestrator,
	}
}

func (service *EncounterCompletionService) GetById(id int) (*model.EncounterCompletion, error) {
	completion, err := service.EncounterCompletionRepo.GetById(id)
	if err != nil {
		return nil, err
	}
	return completion, nil
}

func (service *EncounterCompletionService) GetPagedByUser(userId string) ([]*model.EncounterCompletion, error) {
	if encounterCompletions, err := service.EncounterCompletionRepo.GetPagedByUser(userId); err == nil {
		return encounterCompletions, nil
	}
	return nil, fmt.Errorf("Error finding encounter completions")
}

func (service *EncounterCompletionService) FinishEncounter(userId, encounterId int) (*model.EncounterCompletion, error) {

	encounterCompletion, err := service.EncounterCompletionRepo.GetByUserAndEncounter(userId, encounterId)

	if err != nil {
		return nil, err
	}

	if encounterCompletion == nil {
		return nil, fmt.Errorf("Encounter completion not found for user and encounter")
	}

	encounterCompletion.Status = model.CompletionStatusAwaitingFinish

	if err := service.EncounterCompletionRepo.Update(encounterCompletion); err != nil {
		return encounterCompletion, err
	}

	err = service.orchestrator.Start(encounterCompletion)
	if err != nil {
		encounterCompletion.Status = model.CompletionStatusProgressing
		_ = service.EncounterCompletionRepo.Update(encounterCompletion)
		return nil, err
	}

	return encounterCompletion, nil
}
func (service *EncounterCompletionService) StartEncounter(userId, encounterId int) (*model.EncounterCompletion, error) {
	encounter, err := service.EncounterService.GetById(encounterId)
	if err != nil {
		return nil, err
	}
	encounterCompletion := model.EncounterCompletion{
		UserId:        userId,
		EncounterId:   encounterId,
		LastUpdatedAt: time.Now(),
		Xp:            encounter.Xp,
		Status:        model.CompletionStatusStarted,
		Encounter:     encounter,
	}

	if service.EncounterCompletionRepo.HasUserStartedEncounter(userId, encounterId) {
		return nil, fmt.Errorf("Error: Already started encounter")
	}
	result, err := service.EncounterCompletionRepo.Create(&encounterCompletion)
	if err != nil {
		return nil, err
	}
	return result, err
}

func (service *EncounterCompletionService) Finish(completion *model.EncounterCompletion) error {
	encounterCompletion, err := service.GetById(completion.Id)
	if err != nil {
		return err
	}
	encounterCompletion.Status = model.CompletionStatusCompleted
	return service.EncounterCompletionRepo.Update(encounterCompletion)
}
func (service *EncounterCompletionService) NotFinish(completion *model.EncounterCompletion) error {
	encounterCompletion, err := service.GetById(completion.Id)
	if err != nil {
		return err
	}
	encounterCompletion.Status = model.CompletionStatusProgressing
	return service.EncounterCompletionRepo.Update(encounterCompletion)
}
