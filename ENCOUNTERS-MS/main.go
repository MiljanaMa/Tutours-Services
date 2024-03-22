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

	router.HandleFunc("/encounters", handler.GetApproved).Methods("GET")
	router.HandleFunc("/tourist/encounter/{id}", handlerCompletion.GetPagedByUser).Methods("GET")
	router.HandleFunc("/tourist/encounter/finishEncounter/{id}", handlerCompletion.FinishEncounter).Methods("GET")
	router.HandleFunc("/keypointencounter/{keypointid}", keypointEncHandler.GetPagedByKeypoint).Methods("GET")
	router.HandleFunc("/keypointencounter/create", keypointEncHandler.Create).Methods("POST")
	router.HandleFunc("/keypointencounter/update", keypointEncHandler.Update).Methods("PUT")
	router.HandleFunc("/keypointencounter/delete", keypointEncHandler.Delete).Methods("DELETE")

	fmt.Println("Server is starting...")
	log.Fatal(http.ListenAndServe(":8083", router))
}
func main() {

	/*uuid1 := uuid.New()
	uuid2 := uuid.New()
	uuid3 := uuid.New()
	fmt.Println("k1:", uuid1)
	fmt.Println("k2:", uuid2)
	fmt.Println("k3", uuid3)*/

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
