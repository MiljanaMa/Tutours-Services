package main

import (
	"log"
	"net/http"
	"tours/app"
	"tours/handler"
	"tours/repo"
	"tours/service"

	"github.com/gorilla/mux"
)

func main() {

	db := app.InitDB()
	app.ExecuteMigrations(db)

	// Tours setup
	tourRepo := &repo.TourRepository{DatabaseConnection: db}
	tourService := &service.TourService{TourRepository: tourRepo}
	tourHandler := &handler.TourHandler{TourService: tourService}

	// Keypoints setup
	keypointRepo := &repo.KeypointRepository{DatabaseConnection: db}
	keypointService := &service.KeypointService{KeypointRepository: keypointRepo}
	keypointHandler := &handler.KeypointHandler{KeypointService: keypointService}

	// Tourist positions setup
	touristPositionRepo := &repo.TouristPositionRepository{DatabaseConnection: db}
	touristPositionService := &service.TouristPositionService{TouristPositionRepository: touristPositionRepo}
	touristPositionHandler := &handler.TouristPositionHandler{TouristPositionService: touristPositionService}

	// Tourist review setup
	tourReviewRepo := &repo.TourReviewRepository{DatabaseConnection: db}
	tourReviewService := &service.TourReviewService{TourReviewRepository: tourReviewRepo}
	tourReviewHandler := &handler.TourReviewHandler{TourReviewService: tourReviewService}

	router := mux.NewRouter()

	app.SetupTourRoutes(router, tourHandler)
	app.SetupKeypointRoutes(router, keypointHandler)
	app.SetupTouristPositionRoutes(router, touristPositionHandler)
	app.SetupTourReviewRoutes(router, tourReviewHandler)

	log.Fatal(http.ListenAndServe(app.Port, router))
}
