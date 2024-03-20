package main

import (
	"ENCOUNTERS-MS/handler"
	"ENCOUNTERS-MS/model"
	"ENCOUNTERS-MS/repo"
	"ENCOUNTERS-MS/service"
	"fmt"
	"github.com/gorilla/mux"
	"gorm.io/gorm"
	"log"
	"net/http"
)
import "gorm.io/driver/postgres"

func initDB() *gorm.DB {
	connection_url := "postgres://postgres:super@localhost:5432"
	database, err := gorm.Open(postgres.Open(connection_url), &gorm.Config{})

	if err != nil {
		log.Fatal(err)
		return nil
	}

	models := []interface{}{model.Encounter{}}

	for _, m := range models {
		if err := database.AutoMigrate(m); err != nil {
			log.Fatal(err)
			return nil
		}
	}

	return database

}
func startServer(handler *handler.EncounterHandler) {
	router := mux.NewRouter().StrictSlash(true)

	// GET
	router.HandleFunc("/encounters", handler.GetApproved).Methods("GET")
	router.HandleFunc("/tourist-created-encounters", handler.GetTouristCreatedEncounters).Methods("GET")
	router.HandleFunc("/encounters/nearby/{userId}", handler.GetNearby).Methods("GET")
	router.HandleFunc("/encounters/nearby-by-type/{userId}", handler.GetNearbyByType).Methods("GET")

	// POST
	router.HandleFunc("/encounters", handler.Create).Methods("POST")

	// DELETE
	router.HandleFunc("/encounters/{id}", handler.Delete).Methods("DELETE")

	fmt.Println("Server is starting...")
	log.Fatal(http.ListenAndServe(":8083", router))
}
func main() {
	db := initDB()

	if db == nil {
		return
	}

	repo := &repo.EncounterRepository{db}
	service := &service.EncounterService{repo}
	handler := &handler.EncounterHandler{service}

	startServer(handler)
}
