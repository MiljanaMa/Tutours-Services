package model

import "time"

type TouristPosition struct {
	Id        int `gorm:"primaryKey"`
	UserId    int
	Latitude  float64
	Longitude float64
	UpdatedAt time.Time
}
