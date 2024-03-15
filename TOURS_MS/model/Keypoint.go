package model

type Keypoint struct {
	Id          int `gorm:"primaryKey"`
	TourId      int
	Name        string
	Latitude    float64
	Longitude   float64
	Description string
	Position    int
	Image       string
	Secret      string
}
