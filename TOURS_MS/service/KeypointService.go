package service

import (
	"fmt"
	"tours/model"
	"tours/repo"
)

type KeypointService struct {
	KeypointRepository *repo.KeypointRepository
}

func (service *KeypointService) GetById(id string) (*model.Keypoint, error) {
	keypoint, err := service.KeypointRepository.GetById(id)
	if err != nil {
		return nil, fmt.Errorf(fmt.Sprintf("menu item with id %s not found", id))
	}
	return &keypoint, nil
}

func (service *KeypointService) GetByTourId(tourId string) ([]model.Keypoint, error) {
	keypoints, err := service.KeypointRepository.GetByTourId(tourId)
	if err != nil {
		return nil, fmt.Errorf("failed to retrieve keypoints for tour with ID %s: %w", tourId, err)
	}
	if len(keypoints) == 0 {
		return nil, fmt.Errorf("no keypoints found for tour with ID %s", tourId)
	}
	return keypoints, nil
}

func (service *KeypointService) Create(kp *model.Keypoint) (*model.Keypoint, error) {
	keypoint, err := service.KeypointRepository.Create(kp)
	if err != nil {
		return nil, fmt.Errorf(fmt.Sprintf("Not created"))
	}
	return &keypoint, nil
}
