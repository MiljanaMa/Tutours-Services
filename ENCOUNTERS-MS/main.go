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
	}

	for _, m := range models {
		if err := database.AutoMigrate(m); err != nil {
			log.Fatal(err)
			return nil
		}
	}

	return database

}
func startServer(handler *handler.EncounterHandler, handlerCompletion *handler.EncounterCompletionHandler) {
	router := mux.NewRouter().StrictSlash(true)

	router.HandleFunc("/encounters", handler.GetApproved).Methods("GET")
	router.HandleFunc("/tourist/encounter/{id}", handlerCompletion.GetPagedByUser).Methods("GET")

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

	startServer(encounterHandler, completionHandler)
}

// started kod turiste su mozda vezani za completition,
// mada pogledaj opet
