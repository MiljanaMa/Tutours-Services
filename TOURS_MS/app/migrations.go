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
