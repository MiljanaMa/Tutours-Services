package main

import (
	"ENCOUNTERS-MS/handler"
	"ENCOUNTERS-MS/model"
	"ENCOUNTERS-MS/repo"
	"ENCOUNTERS-MS/service"
	"fmt"
	"log"
	"net/http"

	"github.com/gorilla/mux"
	"gorm.io/driver/postgres"
	"gorm.io/gorm"
)

func initDB() *gorm.DB {
	connection_url := "postgres://postgres:super@localhost:5432"
	database, err := gorm.Open(postgres.Open(connection_url), &gorm.Config{})

	if err != nil {
		log.Fatal(err)
		return nil
	}

	models := []interface{}{
		model.Encounter{},
		model.EncounterCompletion{},
		model.KeypointEncounter{},
	}

	for _, m := range models {
		if err := database.AutoMigrate(m); err != nil {
			log.Fatal(err)
			return nil
		}
	}

	return database

}
func startServer(handler *handler.EncounterHandler, handlerCompletion *handler.EncounterCompletionHandler, keypointEncHandler *handler.KeypointEncounterHandler) {
	router := mux.NewRouter().StrictSlash(true)

	//ENCOUNTER
	router.HandleFunc("/encounters", handler.GetApproved).Methods("GET")                                         // tested
	router.HandleFunc("/tourist-created-encounters", handler.GetTouristCreatedEncounters).Methods("GET")         // tested
	router.HandleFunc("/encounters/nearby/{userId}", handler.GetNearby).Methods("GET")                           // tested
	router.HandleFunc("/encounters/nearby-by-type/{userId}", handler.GetNearbyByType).Methods("GET")             // tested
	router.HandleFunc("/encounters/get-by-user/{userId}", handler.GetByUser).Methods("GET")                      // tested
	router.HandleFunc("/encounters/get-approved-by-status/{status}", handler.GetApprovedByStatus).Methods("GET") // tested
	router.HandleFunc("/encounters", handler.Create).Methods("POST")                                             // tested
	router.HandleFunc("/encounters", handler.Update).Methods("PUT")                                              // tested
	router.HandleFunc("/encounters/approve", handler.Approve).Methods("PUT")
	router.HandleFunc("/encounters/decline", handler.Decline).Methods("PUT")
	router.HandleFunc("/encounters/{id}", handler.Delete).Methods("DELETE") // tested

	//ENCOUNTER COMPLETION
	router.HandleFunc("/tourist/encounter/{id}", handlerCompletion.GetPagedByUser).Methods("GET")
	router.HandleFunc("/tourist/encounter/finishEncounter/{id}", handlerCompletion.FinishEncounter).Methods("GET")

	//KEYPOINT ENCOUNTER
	router.HandleFunc("/keypointencounter/{keypointid}", keypointEncHandler.GetPagedByKeypoint).Methods("GET")
	router.HandleFunc("/keypointencounter/create", keypointEncHandler.Create).Methods("POST")
	router.HandleFunc("/keypointencounter/update", keypointEncHandler.Update).Methods("PUT")
	router.HandleFunc("/keypointencounter/delete", keypointEncHandler.Delete).Methods("DELETE")

	fmt.Println("Server is starting...")
	log.Fatal(http.ListenAndServe(":8083", router))
}
func main() {

	db := initDB()

	if db == nil {
		return
	}

	encounterRepo := &repo.EncounterRepository{db}
	encounterService := &service.EncounterService{encounterRepo}
	encounterHandler := &handler.EncounterHandler{encounterService}

	completionRepo := &repo.EncounterCompletionRepository{db}
	completionService := &service.EncounterCompletionService{completionRepo}
	completionHandler := &handler.EncounterCompletionHandler{completionService}

	keypointEncRepo := &repo.KeypointEncounterRepository{db}
	keypointEncService := &service.KeypointEncounterService{keypointEncRepo}
	keypointEncHandler := &handler.KeypointEncounterHandler{keypointEncService}

	startServer(encounterHandler, completionHandler, keypointEncHandler)
}
