package model

import "encoding/json"

type Keypoint struct {
	Id          int     `bson:"_id,omitempty" json:"id"`
	TourId      int     `bson:"tour_id,omitempty" json:"tour_id"`
	Name        string  `bson:"name,omitempty" json:"name"`
	Latitude    float64 `bson:"latitude,omitempty" json:"latitude"`
	Longitude   float64 `bson:"longitude,omitempty" json:"longitude"`
	Description string  `bson:"description,omitempty" json:"description"`
	Position    int     `bson:"position,omitempty" json:"position"`
	Image       string  `bson:"image,omitempty" json:"image"`
	Secret      string  `bson:"secret,omitempty" json:"secret"`
}

func (k *Keypoint) UnmarshalJSON(data []byte) error {

	var temp struct {
		Id          int
		TourId      int
		Name        string
		Latitude    float64
		Longitude   float64
		Description string
		Position    int
		Image       string
		Secret      string
	}

	if err := json.Unmarshal(data, &temp); err != nil {
		return err
	}

	k.Id = temp.Id
	k.TourId = temp.TourId
	k.Name = temp.Name
	k.Latitude = temp.Latitude
	k.Longitude = temp.Longitude
	k.Description = temp.Description
	k.Position = temp.Position
	k.Image = temp.Image
	k.Secret = temp.Secret

	return nil
}

func (k *Keypoint) MarshalJSON() ([]byte, error) {
	return json.Marshal(struct {
		Id          int
		TourId      int
		Name        string
		Latitude    float64
		Longitude   float64
		Description string
		Position    int
		Image       string
		Secret      string
	}{
		Id:          k.Id,
		TourId:      k.TourId,
		Name:        k.Name,
		Latitude:    k.Latitude,
		Longitude:   k.Longitude,
		Description: k.Description,
		Position:    k.Position,
		Image:       k.Image,
		Secret:      k.Secret,
	})
}
