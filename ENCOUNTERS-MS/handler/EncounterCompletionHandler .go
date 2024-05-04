package handler

import (
	"ENCOUNTERS-MS/model"
	"ENCOUNTERS-MS/proto/encounter"
	"ENCOUNTERS-MS/service"
	"context"
	"strconv"

	"google.golang.org/protobuf/types/known/timestamppb"
)

type EncounterCompletionHandler struct {
	encounter.UnimplementedEncounterCompletionServiceServer
	EncounterCompletionService *service.EncounterCompletionService
}

func mapToRPCStatus(status model.EncounterCompletionStatus) encounter.EncounterCompletionStatus {
	switch status {
	case model.CompletionStatusStarted:
		return encounter.EncounterCompletionStatus_STARTED
	case model.CompletionStatusProgressing:
		return encounter.EncounterCompletionStatus_PROGRESSING
	case model.CompletionStatusCompleted:
		return encounter.EncounterCompletionStatus_COMPLETED
	default:
		return encounter.EncounterCompletionStatus_FAILED
	}
}
func modelToRPC(completion *model.EncounterCompletion) *encounter.EncounterCompletion {
	return &encounter.EncounterCompletion{
		Id:            int64(completion.Id),
		UserId:        int64(completion.UserId),
		EncounterId:   int64(completion.EncounterId),
		LastUpdatedAt: timestamppb.New(completion.LastUpdatedAt),
		Xp:            int64(completion.Xp),
		Status:        mapToRPCStatus(completion.Status),
		Encounter:     ModelToRPC(completion.Encounter),
	}
}
func toRPCCompletions(encounters []*model.EncounterCompletion) *encounter.EncounterCompletionsResponse {
	result := make([]*encounter.EncounterCompletion, len(encounters))
	for i, e := range encounters {
		result[i] = modelToRPC(e)
	}
	return &encounter.EncounterCompletionsResponse{EncounterCompletions: result}
}

func (handler *EncounterCompletionHandler) GetEncounterCompletionByUser(ctx context.Context, request *encounter.UserIdRequest) (*encounter.EncounterCompletionsResponse, error) {
	userId := strconv.Itoa(int(request.UserId))

	encounterCompletions, err := handler.EncounterCompletionService.GetPagedByUser(userId)

	if err != nil {
		return nil, err
	}

	return toRPCCompletions(encounterCompletions), nil
}
func (handler *EncounterCompletionHandler) FinishEncounter(ctx context.Context, request *encounter.UserAndIdRequest) (*encounter.EncounterCompletion, error) {

	encounterId := strconv.Itoa(int(request.Id))
	userId := strconv.Itoa(int(request.UserId))

	result, err := handler.EncounterCompletionService.FinishEncounter(userId, encounterId)
	if err != nil {
		return nil, err
	}

	return modelToRPC(result), nil

}
