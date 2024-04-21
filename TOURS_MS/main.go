package main

import (
	"log"
	"net/http"
	"os"
	"tours/app"
	"tours/handler"
	"tours/repo"
	"tours/service"

	"github.com/gorilla/mux"
)

func main() {
	app.Init()
	client := app.InitDB()
	storeLogger := log.New(os.Stdout, "[patient-store] ", log.LstdFlags)
	//app.InsertInfo(client)

	// Tours setup
	tourRepo := &repo.TourRepository{Cli: client, Logger: storeLogger}
	tourService := &service.TourService{TourRepository: tourRepo}
	tourHandler := &handler.TourHandler{TourService: tourService}

	// Keypoints setup
	keypointRepo := &repo.KeypointRepository{Cli: client, Logger: storeLogger}
	keypointService := &service.KeypointService{KeypointRepository: keypointRepo}
	keypointHandler := &handler.KeypointHandler{KeypointService: keypointService}
	/*
		// Tourist positions setup
		touristPositionRepo := &repo.TouristPositionRepository{DatabaseConnection: db}
		touristPositionService := &service.TouristPositionService{TouristPositionRepository: touristPositionRepo}
		touristPositionHandler := &handler.TouristPositionHandler{TouristPositionService: touristPositionService}
	*/
	// Tourist review setup
	tourReviewRepo := &repo.TourReviewRepository{Cli: client, Logger: storeLogger}
	tourReviewService := &service.TourReviewService{TourReviewRepository: tourReviewRepo}
	tourReviewHandler := &handler.TourReviewHandler{TourReviewService: tourReviewService}

	router := mux.NewRouter()

	app.SetupTourRoutes(router, tourHandler)
	app.SetupKeypointRoutes(router, keypointHandler)
	//app.SetupTouristPositionRoutes(router, touristPositionHandler)
	app.SetupTourReviewRoutes(router, tourReviewHandler)

	log.Fatal(http.ListenAndServe(app.Port, router))
}
