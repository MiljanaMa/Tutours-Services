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
	router.HandleFunc("/encounters", handler.GetApproved).Methods("GET")                                         // tested
	router.HandleFunc("/tourist-created-encounters", handler.GetTouristCreatedEncounters).Methods("GET")         // tested
	router.HandleFunc("/encounters/nearby/{userId}", handler.GetNearby).Methods("GET")                           // tested
	router.HandleFunc("/encounters/nearby-by-type/{userId}", handler.GetNearbyByType).Methods("GET")             // tested
	router.HandleFunc("/encounters/get-by-user/{userId}", handler.GetByUser).Methods("GET")                      // tested
	router.HandleFunc("/encounters/get-approved-by-status/{status}", handler.GetApprovedByStatus).Methods("GET") // tested

	// POST
	router.HandleFunc("/encounters", handler.Create).Methods("POST") // tested

	// PUT
	router.HandleFunc("/encounters", handler.Update).Methods("PUT") // tested
	router.HandleFunc("/encounters/approve", handler.Approve).Methods("PUT")
	router.HandleFunc("/encounters/decline", handler.Decline).Methods("PUT")

	// DELETE
	router.HandleFunc("/encounters/{id}", handler.Delete).Methods("DELETE") // tested

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
