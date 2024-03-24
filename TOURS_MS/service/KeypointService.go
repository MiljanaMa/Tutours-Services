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
		return nil, fmt.Errorf("keypoint with id %s not found", id)
	}
	return &keypoint, nil
}

func (service *KeypointService) GetByTourId(tourId string) ([]model.Keypoint, error) {
	keypoints, err := service.KeypointRepository.GetByTourId(tourId)
	if err != nil {
		return nil, fmt.Errorf("failed to retrieve keypoints for tour with id %s: %w", tourId, err)
	}
	if len(keypoints) == 0 {
		return []model.Keypoint{}, nil
	}
	return keypoints, nil
}

func (service *KeypointService) Create(kp *model.Keypoint) (*model.Keypoint, error) {
	keypoint, err := service.KeypointRepository.Create(kp)
	if err != nil {
		return nil, fmt.Errorf("error while creating keypoint")
	}
	return &keypoint, nil
}

func (service *KeypointService) Update(kp *model.Keypoint) (*model.Keypoint, error) {
	keypoint, err := service.KeypointRepository.Update(kp)
	if err != nil {
		return nil, fmt.Errorf("error while updating keypoint")
	}
	return &keypoint, nil
}

func (service *KeypointService) Delete(id string) error {
	err := service.KeypointRepository.Delete(id)
	if err != nil {
		return fmt.Errorf("error while deleting keypoint")
	}
	return nil
}
