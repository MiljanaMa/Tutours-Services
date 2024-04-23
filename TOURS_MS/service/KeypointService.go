package service

import (
	"fmt"
	"tours/model"
	"tours/repo"
)

type KeypointService struct {
	KeypointRepository *repo.KeypointRepository
}

func (service *KeypointService) GetById(id int) (model.Keypoint, error) {
	var keypoint model.Keypoint
	keypoint, err := service.KeypointRepository.GetById(id)
	if err != nil {
		return keypoint, err
	}
	return keypoint, nil
}

func (service *KeypointService) GetByTourId(tourId int) ([]model.Keypoint, error) {
	keypoints, err := service.KeypointRepository.GetByTourId(tourId)
	if err != nil {
		return nil, fmt.Errorf("failed to retrieve keypoints for tour with id %d: %w", tourId, err)
	}
	if len(keypoints) == 0 {
		return []model.Keypoint{}, nil
	}
	return keypoints, nil
}

func (service *KeypointService) Create(kp *model.Keypoint) (*model.Keypoint, error) {
	keypoint, err := service.KeypointRepository.Create(kp)
	if err != nil {
		return nil, fmt.Errorf(err.Error())
	}
	return &keypoint, nil
}

func (service *KeypointService) Delete(id int) error {
	err := service.KeypointRepository.Delete(id)
	if err != nil {
		return err
	}
	return nil
}

func (service *KeypointService) Update(keypoint *model.Keypoint) (model.Keypoint, error) {
	updatedKeypoint, err := service.KeypointRepository.Update(keypoint)
	if err != nil {
		return updatedKeypoint, err
	}
	return updatedKeypoint, nil
}
