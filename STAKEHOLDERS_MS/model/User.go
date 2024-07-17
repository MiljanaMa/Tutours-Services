package model

import UserRole "stakeholder/model/enum"

type User struct {
	Id                int `gorm:"primaryKey;autoIncrement"`
	Username          string
	Password          string
	Role              UserRole.UserRole
	IsActive          bool
	Email             string
	IsBlocked         bool
	IsEnabled         bool
	VerificationToken string
}
