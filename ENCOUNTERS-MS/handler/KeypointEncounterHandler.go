package handler

import (
	"ENCOUNTERS-MS/model"
	"ENCOUNTERS-MS/proto/encounter"
	"ENCOUNTERS-MS/service"
	"context"
	"strconv"
)

type KeypointEncounterHandler struct {
	encounter.UnimplementedKeypointEncounterServiceServer
	KeypointEncounterService *service.KeypointEncounterService
}

func keyPointToRPC(keypointEncounter *model.KeypointEncounter) *encounter.KeypointEncounter {
	return &encounter.KeypointEncounter{
		Id:          int64(keypointEncounter.Id),
		EncounterId: int64(keypointEncounter.EncounterId),
		Encounter:   ModelToRPC(keypointEncounter.Encounter),
		KeyPointId:  int64(keypointEncounter.KeyPointId),
		IsRequired:  keypointEncounter.IsRequired,
	}
}
func rpcToKeyPoint(keypointEncounter *encounter.KeypointEncounter) *model.KeypointEncounter {
	return &model.KeypointEncounter{
		Id:          int(keypointEncounter.Id),
		EncounterId: int(keypointEncounter.EncounterId),
		Encounter:   rpcToModel(keypointEncounter.Encounter),
		KeyPointId:  int(keypointEncounter.KeyPointId),
		IsRequired:  keypointEncounter.IsRequired,
	}
}
func toRPCKeypoints(keypoints []*model.KeypointEncounter) *encounter.KeypointEncounterResponse {
	result := make([]*encounter.KeypointEncounter, len(keypoints))
	for i, e := range keypoints {
		result[i] = keyPointToRPC(e)
	}
	return &encounter.KeypointEncounterResponse{KeypointEncounters: result}
}

func (handler *KeypointEncounterHandler) GetByKeypoint(ctx context.Context, request *encounter.IdRequest) (*encounter.KeypointEncounterResponse, error) {
	keypointId := strconv.Itoa(int(request.Id))
	result, err := handler.KeypointEncounterService.GetPagedByKeypoint(keypointId)
	if err != nil {
		return nil, err
	}

	return toRPCKeypoints(result), nil
}

func (handler *KeypointEncounterHandler) Create(ctx context.Context, request *encounter.KeypointEncounter) (*encounter.KeypointEncounter, error) {

	keypointEncounter := rpcToKeyPoint(request)

	result, err := handler.KeypointEncounterService.Create(keypointEncounter)

	if err != nil {
		return nil, err
	}

	return keyPointToRPC(result), nil
}

func (handler *KeypointEncounterHandler) Update(ctx context.Context, request *encounter.KeypointEncounter) (*encounter.KeypointEncounter, error) {
	keypointEncounter := rpcToKeyPoint(request)

	result, err := handler.KeypointEncounterService.Update(keypointEncounter)
	if err != nil {
		return nil, err
	}
	return keyPointToRPC(result), nil
}

func (handler *KeypointEncounterHandler) Delete(ctx context.Context, request *encounter.IdRequest) (*encounter.EmptyResponse, error) {
	id := strconv.Itoa(int(request.Id))

	err := handler.KeypointEncounterService.Delete(id)
	if err != nil {
		return nil, err
	}

	return &encounter.EmptyResponse{}, nil
}
