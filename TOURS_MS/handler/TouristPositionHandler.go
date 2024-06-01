package handler

import (
	"context"
	"google.golang.org/protobuf/types/known/timestamppb"
	"time"
	"tours/model"
	"tours/proto/tour"
	"tours/service"
)

type TouristPositionHandler struct {
	tour.UnimplementedTouristPositionServiceServer
	TouristPositionService *service.TouristPositionService
}

func modelTogRPC(p *model.TouristPosition) *tour.TouristPosition {
	return &tour.TouristPosition{
		Id:        int64(p.Id),
		UserId:    int64(p.UserId),
		Latitude:  float32(p.Latitude),
		Longitude: float32(p.Longitude),
		UpdatedAt: timestamppb.New(p.UpdatedAt),
	}

}
func gRPCToModel(p *tour.TouristPosition) *model.TouristPosition {
	return &model.TouristPosition{
		Id:        int(p.Id),
		UserId:    int(p.UserId),
		Latitude:  float64(p.Latitude),
		Longitude: float64(p.Longitude),
		UpdatedAt: time.Time{},
	}
}
func (handler *TouristPositionHandler) GetById(ctx context.Context, request *tour.IdRequest) (*tour.TouristPosition, error) {
	id := request.Id

	position, err := handler.TouristPositionService.GetById(int(id))
	if err != nil {
		return nil, err
	}
	return modelTogRPC(&position), nil

}

func (handler *TouristPositionHandler) GetByUserId(ctx context.Context, request *tour.IdRequest) (*tour.TouristPosition, error) {
	id := request.Id
	position, err := handler.TouristPositionService.GetByUserId(int(id))
	if err != nil {
		return nil, err
	}
	return modelTogRPC(&position), nil

}

func (handler *TouristPositionHandler) Create(ctx context.Context, request *tour.TouristPosition) (*tour.TouristPosition, error) {

	tp := gRPCToModel(request)
	position, err := handler.TouristPositionService.Create(tp)
	if err != nil {
		return nil, err
	}
	return modelTogRPC(position), nil
}

func (handler *TouristPositionHandler) Update(ctx context.Context, request *tour.TouristPosition) (*tour.TouristPosition, error) {
	tp := gRPCToModel(request)
	updatedPosition, err := handler.TouristPositionService.Update(tp)
	if err != nil {
		return nil, err
	}
	return modelTogRPC(&updatedPosition), nil
}
