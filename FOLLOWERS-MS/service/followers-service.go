package service

import "FOLLOWERS-MS/repo"

type FollowerService struct {
	FollowerRepository *repo.FollowerRepository
}

func (service *FollowerService) Follow(id1, id2 int) error {
	err := service.FollowerRepository.CreateFollowingCon(id1, id2)
	return err
}

func (service *FollowerService) IsFollowing(id1, id2 int) (bool, error) {
	return service.FollowerRepository.IsFollowing(id1, id2)
}

func (service *FollowerService) GetRecommendations(id int) ([]int, error) {
	return service.FollowerRepository.GetRecommendation(id)
}
