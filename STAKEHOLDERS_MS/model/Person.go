package model

type Person struct {
	Id           int `gorm:"primaryKey;autoIncrement"`
	UserId       int
	Name         string
	Surname      string
	Email        string
	ProfileImage string
	Biography    string
	Quote        string
	XP           int
	Level        int
}
