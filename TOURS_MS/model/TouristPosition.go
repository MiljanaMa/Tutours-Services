package model

import (
	"encoding/json"
	"time"
)

type TouristPosition struct {
	Id        int       `bson:"_id,omitempty" json:"id"`
	UserId    int       `bson:"user_id,omitempty" json:"user_id"`
	Latitude  float64   `bson:"latitude,omitempty" json:"latitude"`
	Longitude float64   `bson:"longitude,omitempty" json:"longitude"`
	UpdatedAt time.Time `bson:"updated_at,omitempty" json:"updated_at"`
}

func (t *TouristPosition) UnmarshalJSON(data []byte) error {

	var temp struct {
		Id        int
		UserId    int
		Latitude  float64
		Longitude float64
		UpdatedAt time.Time
	}

	if err := json.Unmarshal(data, &temp); err != nil {
		return err
	}

	t.Id = temp.Id
	t.UserId = temp.UserId
	t.Latitude = temp.Latitude
	t.Longitude = temp.Longitude
	t.UpdatedAt = temp.UpdatedAt

	return nil
}

func (t *TouristPosition) MarshalJSON() ([]byte, error) {
	return json.Marshal(struct {
		Id        int
		UserId    int
		Latitude  float64
		Longitude float64
		UpdatedAt time.Time
	}{
		Id:        t.Id,
		UserId:    t.UserId,
		Latitude:  t.Latitude,
		Longitude: t.Longitude,
		UpdatedAt: t.UpdatedAt,
	})
}
