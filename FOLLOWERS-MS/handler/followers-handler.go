package handler

import (
	"FOLLOWERS-MS/proto/follower"
	"FOLLOWERS-MS/service"
	"context"
	"fmt"
)

type FollowersHandler struct {
	follower.UnimplementedFollowerServiceServer
	FollowerService *service.FollowerService
}

func (handler *FollowersHandler) toInt64Array(intArray []int) []int64 {
	result := make([]int64, len(intArray))
	for i, e := range intArray {
		result[i] = int64(e)
	}
	return result
}

func (handler *FollowersHandler) Follow(ctx context.Context, request *follower.MultiIdRequest) (*follower.EmptyResponse, error) {
	id1 := int(request.Id1)
	id2 := int(request.Id2)

	var err = handler.FollowerService.Follow(id1, id2)
	if err == nil {
		return &follower.EmptyResponse{}, nil
	}
	fmt.Println(err)
	return &follower.EmptyResponse{}, err

}

func (handler *FollowersHandler) IsFollowing(ctx context.Context, request *follower.MultiIdRequest) (*follower.IsFollowingResponse, error) {

	id1 := int(request.Id1)
	id2 := int(request.Id2)

	result, err := handler.FollowerService.IsFollowing(id1, id2)
	if err != nil {
		fmt.Println(err)
		return nil, err
	}
	return &follower.IsFollowingResponse{IsFollowing: result}, nil
}

func (handler *FollowersHandler) GetRecommendations(ctx context.Context, request *follower.Request) (*follower.MultiIdResponse, error) {

	id := int(request.Id)
	result, err := handler.FollowerService.GetRecommendations(id)

	if err != nil {
		fmt.Println(err)
		return nil, err
	}

	return &follower.MultiIdResponse{Ids: handler.toInt64Array(result)}, nil
}

func (handler *FollowersHandler) Unfollow(ctx context.Context, request *follower.MultiIdRequest) (*follower.EmptyResponse, error) {
	id1 := int(request.Id1)
	id2 := int(request.Id2)

	var err = handler.FollowerService.Unfollow(id1, id2)
	if err == nil {
		return &follower.EmptyResponse{}, nil
	}
	fmt.Println(err)
	return nil, err

}

func (handler *FollowersHandler) GetFollowing(ctx context.Context, request *follower.Request) (*follower.MultiIdResponse, error) {

	id := int(request.Id)

	result, err := handler.FollowerService.GetFollowings(id)

	if err != nil {
		fmt.Println(err)
		return nil, err
	}
	return &follower.MultiIdResponse{Ids: handler.toInt64Array(result)}, nil
}

func (handler *FollowersHandler) GetFollowers(ctx context.Context, request *follower.Request) (*follower.MultiIdResponse, error) {
	id := int(request.Id)

	result, err := handler.FollowerService.GetFollowers(id)

	if err != nil {
		fmt.Println(err)
		return nil, err
	}

	return &follower.MultiIdResponse{Ids: handler.toInt64Array(result)}, nil
}
