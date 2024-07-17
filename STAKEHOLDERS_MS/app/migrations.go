package app

import (
	"gorm.io/gorm"
	"stakeholder/model"
)

func MigrateDatabase(database *gorm.DB) {
	migrator := database.Migrator()

	if !migrator.HasTable(&model.User{}) {
		migrator.CreateTable(&model.User{})
	}
	if !migrator.HasTable(&model.Person{}) {
		migrator.CreateTable(&model.Person{})
	}
}
func ExecuteMigrations(database *gorm.DB) {

	database.AutoMigrate(&model.User{})
	database.AutoMigrate(&model.Person{})
}
