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

	router := mux.NewRouter()

	app.SetupTourRoutes(router, tourHandler)

	log.Fatal(http.ListenAndServe(app.Port, router))
}
