package model

type Tour struct {
	Id       int `gorm:"primaryKey"`
	Name     string
	Location string
}
