package app

import (
	"tours/model"

	"gorm.io/gorm"
)

func ExecuteMigrations(database *gorm.DB) {
	// tours
	database.AutoMigrate(&model.Tour{})

	//keypoints
	database.AutoMigrate(&model.Keypoint{})

	//tourist positions
	database.AutoMigrate(&model.TouristPosition{})
	database.AutoMigrate(&model.TourReview{})

}
func MigrateDatabase(database *gorm.DB) {
	migrator := database.Migrator()

	// Example: Create a new table
	if !migrator.HasTable(&model.Tour{}) {
		migrator.CreateTable(&model.Tour{})
	}
	if !migrator.HasTable(&model.Keypoint{}) {
		migrator.CreateTable(&model.Keypoint{})
	}
	if !migrator.HasTable(&model.TouristPosition{}) {
		migrator.CreateTable(&model.TouristPosition{})
	}
	if !migrator.HasTable(&model.TourReview{}) {
		migrator.CreateTable(&model.TourReview{})
	}

	// Other migration steps...
}
