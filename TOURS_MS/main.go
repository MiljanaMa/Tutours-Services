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

	router := mux.NewRouter()

	app.SetupTourRoutes(router, tourHandler)
	app.SetupKeypointRoutes(router, keypointHandler)

	log.Fatal(http.ListenAndServe(app.Port, router))
}
