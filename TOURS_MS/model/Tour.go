package model

import (
	"encoding/json"
	"errors"
	"gorm.io/gorm"
	"time"
	"tours/model/enum"
	"tours/model/helper"
)

type Tour struct {
	Id               int `gorm:"primaryKey"`
	UserId           int
	Name             string
	Description      string
	Price            float64
	Duration         int
	Distance         float64
	Difficulty       enum.TourDifficulty
	TransportType    enum.TransportType
	Status           enum.TourStatus
	StatusUpdateTime time.Time
	Tags             helper.ArrayString
	//fali TourEquipments, Keypoints, TourReviews, Bundles
}

func NewTour(name string, description string, price float64, difficulty enum.TourDifficulty,
	tags helper.ArrayString, status enum.TourStatus, userId int, distance float64, duration int, transportType enum.TransportType,
	statusUpdateTime time.Time) *Tour {
	tour := &Tour{
		UserId:           userId,
		Name:             name,
		Description:      description,
		Price:            price,
		Duration:         duration,
		Distance:         distance,
		Difficulty:       difficulty,
		Tags:             tags,
		Status:           status,
		TransportType:    transportType,
		StatusUpdateTime: statusUpdateTime,
		//KeyPoints:     keyPoints,
	}

	return tour
}

func (tour *Tour) CreateID(scope *gorm.DB) error {
	tour.Id = 0
	return nil
}
func (tour *Tour) validate() error {
	if tour.Name == "" {
		return errors.New("invalid name")
	}
	if tour.Description == "" {
		return errors.New("invalid Description")
	}
	if tour.Price < 0 {
		return errors.New("invalid Price")
	}
	if tour.Duration < 0 {
		return errors.New("invalid Duration")
	}
	if tour.Distance < 0 {
		return errors.New("invalid Distance")
	}
	if len(tour.Tags) == 0 {
		return errors.New("not enough Tags")
	}
	if tour.Status == enum.PUBLISHED {
		return errors.New("tour is already published")
	}
	/*if len(tour.KeyPoints) < 2 {
		return errors.New("not enough Key Points")
	}
	*/

	return nil
}
func (t *Tour) IsEmpty() bool {
	return t.Name == ""
}

func (t *Tour) MarshalJSON() ([]byte, error) {
	return json.Marshal(struct {
		Id               int
		UserId           int
		Name             string
		Description      string
		Price            float64
		Duration         int
		Distance         float64
		Difficulty       string
		TransportType    string
		Status           string
		StatusUpdateTime time.Time
		Tags             helper.ArrayString
	}{
		Id:               t.Id,
		UserId:           t.UserId,
		Name:             t.Name,
		Description:      t.Description,
		Price:            t.Price,
		Duration:         t.Duration,
		Distance:         t.Distance,
		Difficulty:       t.Difficulty.ToString(),
		TransportType:    t.TransportType.ToString(),
		Status:           t.Status.ToString(),
		StatusUpdateTime: t.StatusUpdateTime,
		Tags:             t.Tags,
	})
}
func (t *Tour) UnmarshalJSON(data []byte) error {

	var temp struct {
		Id               int
		UserId           int
		Name             string
		Description      string
		Price            float64
		Duration         int
		Distance         float64
		Difficulty       string
		TransportType    string
		Status           string
		StatusUpdateTime time.Time
		Tags             helper.ArrayString
	}

	if err := json.Unmarshal(data, &temp); err != nil {
		return err
	}

	t.Id = temp.Id
	t.UserId = temp.UserId
	t.Name = temp.Name
	t.Description = temp.Description
	t.Price = temp.Price
	t.Duration = temp.Duration
	t.Distance = temp.Distance
	t.Difficulty = enum.FromStringToDifficulty(temp.Difficulty)
	t.TransportType = enum.FromStringToType(temp.TransportType)
	t.Status = enum.FromStringToStatus(temp.Status)
	t.StatusUpdateTime = temp.StatusUpdateTime
	t.Tags = temp.Tags

	return nil
}
