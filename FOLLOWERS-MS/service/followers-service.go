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
	recommendations, err := service.FollowerRepository.GetRecommendation(id)
	if len(recommendations) == 0 {
		return []int{}, err
	}
	return recommendations, err
}

func (service *FollowerService) Unfollow(id1, id2 int) error {
	err := service.FollowerRepository.DeleteFollowingCon(id1, id2)
	return err
}

func (service *FollowerService) GetFollowings(id int) ([]int, error) {
	followings, err := service.FollowerRepository.GetFollowings(id)
	if len(followings) == 0 {
		return []int{}, err
	}
	return followings, err
}

func (service *FollowerService) GetFollowers(id int) ([]int, error) {
	followers, err := service.FollowerRepository.GetFollowers(id)
	if len(followers) == 0 {
		return []int{}, err
	}
	return followers, err
}
