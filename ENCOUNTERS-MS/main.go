package main

import (
	"ENCOUNTERS-MS/handler"
	"ENCOUNTERS-MS/model"
	"ENCOUNTERS-MS/proto/encounter"
	"ENCOUNTERS-MS/repo"
	"ENCOUNTERS-MS/service"
	"fmt"
	"log"
	"net"
	"net/http"
	"os"
	"os/signal"
	"syscall"

	"github.com/gorilla/mux"
	"google.golang.org/grpc"
	"google.golang.org/grpc/reflection"
	"gorm.io/driver/postgres"
	"gorm.io/gorm"
)

func initDB() *gorm.DB {
	connectionUrl :=
		"user=" + os.Getenv("DB_USER_E") +
			" password=" + os.Getenv("DB_PASSWORD_E") +
			" host=" + os.Getenv("DB_HOST_E") +
			" search_path=public" +
			" dbname=" + os.Getenv("DB_DATABASE_E") +
			" port=" + os.Getenv("DB_PORT_E") +
			" sslmode=disable"
	//connectionUrl := "postgres://postgres:super@localhost:5432"
	database, err := gorm.Open(postgres.Open(connectionUrl), &gorm.Config{SkipDefaultTransaction: true})

	if err != nil {
		log.Fatal(err)
		return nil
	}
	/*
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
	*/
	MigrateDatabase(database)
	return database

}
func MigrateDatabase(database *gorm.DB) {
	migrator := database.Migrator()

	// Example: Create a new table
	if !migrator.HasTable(&model.Encounter{}) {
		migrator.CreateTable(&model.Encounter{})
	}
	if !migrator.HasTable(&model.EncounterCompletion{}) {
		migrator.CreateTable(&model.EncounterCompletion{})
	}
	if !migrator.HasTable(&model.KeypointEncounter{}) {
		migrator.CreateTable(&model.KeypointEncounter{})
	}
}
func startServer(handler *handler.EncounterHandler, handlerCompletion *handler.EncounterCompletionHandler, keypointEncHandler *handler.KeypointEncounterHandler) {
	router := mux.NewRouter().StrictSlash(true)

	//KEYPOINT ENCOUNTER
	//router.HandleFunc("/keypointencounter/update", keypointEncHandler.Update).Methods("PUT")
	//router.HandleFunc("/keypointencounter/delete", keypointEncHandler.Delete).Methods("DELETE")

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
	encounterHandler := &handler.EncounterHandler{EncounterService: encounterService}

	completionRepo := &repo.EncounterCompletionRepository{db}
	completionService := &service.EncounterCompletionService{completionRepo}
	completionHandler := &handler.EncounterCompletionHandler{EncounterCompletionService: completionService}

	keypointEncRepo := &repo.KeypointEncounterRepository{db}
	keypointEncService := &service.KeypointEncounterService{keypointEncRepo}
	keypointEncHandler := &handler.KeypointEncounterHandler{KeypointEncounterService: keypointEncService}

	lis, err := net.Listen("tcp", ":8092")
	fmt.Println("Running gRPC on port 8092")
	if err != nil {
		log.Fatalln(err)
	}

	defer func(listener net.Listener) {
		err := listener.Close()
		if err != nil {
			log.Fatalln(err)
		}
	}(lis)

	grpcServer := grpc.NewServer()
	reflection.Register(grpcServer)
	fmt.Println("Registered gRPC server")

	encounter.RegisterEncounterServiceServer(grpcServer, encounterHandler)
	encounter.RegisterEncounterCompletionServiceServer(grpcServer, completionHandler)
	encounter.RegisterKeypointEncounterServiceServer(grpcServer, keypointEncHandler)

	go func() {
		if err := grpcServer.Serve(lis); err != nil {
			log.Fatalln(err)
		}
	}()
	fmt.Println("Serving gRPC")

	stopCh := make(chan os.Signal)
	signal.Notify(stopCh, syscall.SIGTERM)

	<-stopCh

	grpcServer.Stop()
}
