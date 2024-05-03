package handler

import (
	"ENCOUNTERS-MS/model"
	"ENCOUNTERS-MS/proto/encounter"
	"ENCOUNTERS-MS/service"
	"context"
)

type EncounterHandler struct {
	encounter.UnimplementedEncounterServiceServer
	EncounterService *service.EncounterService
}

func mapEncounterType(encounterType model.EncounterType) encounter.EncounterType {
	switch encounterType {
	case model.TypeSocial:
		return encounter.EncounterType_SOCIAL
	case model.TypeMisc:
		return encounter.EncounterType_MISC
	default:
		return encounter.EncounterType_LOCATION
	}
}
func mapEncounterStatus(status model.EncounterStatus) encounter.EncounterStatus {
	switch status {
	case model.StatusActive:
		return encounter.EncounterStatus_ACTIVE
	case model.StatusDraft:
		return encounter.EncounterStatus_DRAFT
	default:
		return encounter.EncounterStatus_ARCHIVED
	}

}
func mapEncounterApprovalStatus(status model.EncounterApprovalStatus) encounter.EncounterApprovalStatus {
	switch status {
	case model.ApprovalAdminApproved:
		return encounter.EncounterApprovalStatus_ADMIN_APPROVED
	case model.ApprovalDeclined:
		return encounter.EncounterApprovalStatus_DECLINED
	case model.ApprovalSystemApproved:
		return encounter.EncounterApprovalStatus_SYSTEM_APPROVED
	default:
		return encounter.EncounterApprovalStatus_PENDING
	}
}

func inverseEncounterType(encounterType encounter.EncounterType) model.EncounterType {
	switch encounterType {
	case encounter.EncounterType_SOCIAL:
		return model.TypeSocial
	case encounter.EncounterType_MISC:
		return model.TypeMisc
	default:
		return model.TypeLocation
	}
}
func inverseEncounterStatus(status encounter.EncounterStatus) model.EncounterStatus {
	switch status {
	case encounter.EncounterStatus_ACTIVE:
		return model.StatusActive
	case encounter.EncounterStatus_DRAFT:
		return model.StatusDraft
	default:
		return model.StatusArchived
	}
}
func inverseEncounterApprovalStatus(status encounter.EncounterApprovalStatus) model.EncounterApprovalStatus {
	switch status {
	case encounter.EncounterApprovalStatus_ADMIN_APPROVED:
		return model.ApprovalAdminApproved
	case encounter.EncounterApprovalStatus_DECLINED:
		return model.ApprovalDeclined
	case encounter.EncounterApprovalStatus_SYSTEM_APPROVED:
		return model.ApprovalSystemApproved
	default:
		return model.ApprovalPending
	}
}

func modelToRPC(e *model.Encounter) *encounter.Encounter {
	return &encounter.Encounter{
		Id:             int64(e.Id),
		UserId:         int64(e.UserId),
		Name:           e.Name,
		Description:    e.Description,
		Latitude:       float32(e.Latitude),
		Longitude:      float32(e.Longitude),
		Xp:             int64(e.Xp),
		Status:         mapEncounterStatus(e.Status),
		Type:           mapEncounterType(e.Type),
		Range:          float32(e.Range),
		Image:          e.Image,
		ImageLatitude:  float32(e.ImageLatitude),
		ImageLongitude: float32(e.ImageLongitude),
		PeopleCount:    int64(e.PeopleCount),
		ApprovalStatus: mapEncounterApprovalStatus(e.ApprovalStatus),
	}
}
func rpcToModel(e *encounter.Encounter) *model.Encounter {
	return &model.Encounter{
		Id:             int(e.Id),
		UserId:         int(e.UserId),
		Name:           e.Name,
		Description:    e.Description,
		Latitude:       float64(e.Latitude),
		Longitude:      float64(e.Longitude),
		Xp:             int(e.Xp),
		Status:         inverseEncounterStatus(e.Status),
		Type:           inverseEncounterType(e.Type),
		Range:          float64(e.Range),
		Image:          e.Image,
		ImageLatitude:  float64(e.ImageLatitude),
		ImageLongitude: float64(e.ImageLongitude),
		PeopleCount:    int(e.PeopleCount),
		ApprovalStatus: inverseEncounterApprovalStatus(e.ApprovalStatus),
	}
}

func toRPCEncounters(encounters []*model.Encounter) *encounter.EncountersResponse {
	result := make([]*encounter.Encounter, len(encounters))
	for i, e := range encounters {
		result[i] = modelToRPC(e)
	}
	return &encounter.EncountersResponse{Encounters: result}
}

func (handler *EncounterHandler) GetApproved(ctx context.Context, request *encounter.EmptyRequest) (*encounter.EncountersResponse, error) {

	encounters, err := handler.EncounterService.GetApproved()
	if err != nil {
		return nil, err
	}
	return toRPCEncounters(encounters), nil

}

func (handler *EncounterHandler) GetTouristCreatedEncounters(ctx context.Context, request *encounter.EmptyRequest) (*encounter.EncountersResponse, error) {
	encounters, err := handler.EncounterService.GetTouristCreatedEncounters()
	if err != nil {
		return nil, err
	}
	return toRPCEncounters(encounters), nil
}

func (handler *EncounterHandler) GetNearby(ctx context.Context, request *encounter.UserIdRequest) (*encounter.EncountersResponse, error) {
	userId := int(request.UserId)

	encounters, err := handler.EncounterService.GetNearby(userId)
	if err != nil {
		return nil, err
	}
	return toRPCEncounters(encounters), nil
}

func (handler *EncounterHandler) GetNearbyByType(ctx context.Context, request *encounter.UserIdRequest) (*encounter.EncountersResponse, error) {

	userId := int(request.UserId)

	encounters, err := handler.EncounterService.GetNearbyHidden(userId)
	if err != nil {
		return nil, err
	}

	return toRPCEncounters(encounters), nil

}

func (handler *EncounterHandler) CreateEncounter(ctx context.Context, request *encounter.Encounter) (*encounter.Encounter, error) {
	e := rpcToModel(request)

	res, err := handler.EncounterService.Create(e)

	if err != nil {
		return nil, err
	}

	return modelToRPC(res), nil

}

func (handler *EncounterHandler) DeleteEncounter(ctx context.Context, request *encounter.IdRequest) (*encounter.EmptyResponse, error) {
	id := int(request.Id)

	if err := handler.EncounterService.Delete(id); err != nil {
		return nil, err
	}

	return &encounter.EmptyResponse{}, nil
}

func (handler *EncounterHandler) GetByUser(ctx context.Context, request *encounter.UserIdRequest) (*encounter.EncountersResponse, error) {
	userId := int(request.UserId)

	encounters, err := handler.EncounterService.GetByUser(userId)
	if err != nil {
		return nil, err
	}
	return toRPCEncounters(encounters), nil

}

func (handler *EncounterHandler) GetApprovedByStatus(ctx context.Context, request *encounter.StatusRequest) (*encounter.EncountersResponse, error) {
	status := request.Status

	encounters, err := handler.EncounterService.GetApprovedByStatus(model.EncounterStatus(status))
	if err != nil {
		return nil, err
	}
	return toRPCEncounters(encounters), nil
}

func (handler *EncounterHandler) Approve(ctx context.Context, request *encounter.Encounter) (*encounter.EmptyResponse, error) {
	e := rpcToModel(request)

	e.ApprovalStatus = model.ApprovalAdminApproved
	_, err := handler.EncounterService.Update(e)
	if err != nil {
		return nil, err
	}
	return &encounter.EmptyResponse{}, nil
}

func (handler *EncounterHandler) Decline(ctx context.Context, request *encounter.Encounter) (*encounter.EmptyResponse, error) {
	e := rpcToModel(request)

	e.ApprovalStatus = model.ApprovalDeclined
	_, err := handler.EncounterService.Update(e)
	if err != nil {
		return nil, err
	}
	return &encounter.EmptyResponse{}, nil
}

func (handler *EncounterHandler) UpdateEncounter(ctx context.Context, request *encounter.Encounter) (*encounter.Encounter, error) {
	e := rpcToModel(request)

	_, err := handler.EncounterService.Update(e)
	if err != nil {
		return nil, err
	}
	return modelToRPC(e), nil
}
