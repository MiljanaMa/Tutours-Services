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

	//objects

}
