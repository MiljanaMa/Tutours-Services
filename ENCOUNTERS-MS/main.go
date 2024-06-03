package main

import (
	"ENCOUNTERS-MS/handler"
	"ENCOUNTERS-MS/model"
	"ENCOUNTERS-MS/proto/encounter"
	"ENCOUNTERS-MS/repo"
	saga "ENCOUNTERS-MS/saga/messaging"
	"ENCOUNTERS-MS/saga/messaging/nats"
	"ENCOUNTERS-MS/service"
	"context"
	"fmt"
	"log"
	"net"
	"os"
	"os/signal"
	"syscall"

	"google.golang.org/grpc/codes"
	"google.golang.org/grpc/metadata"
	"google.golang.org/grpc/status"

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
	//connectionUrl = "postgres://postgres:super@localhost:5432"
	database, err := gorm.Open(postgres.Open(connectionUrl), &gorm.Config{SkipDefaultTransaction: true})
	fmt.Println("Initializing DB with connection URL:", connectionUrl)
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

func JWTInterceptor(ctx context.Context, req interface{}, info *grpc.UnaryServerInfo, handler grpc.UnaryHandler) (interface{}, error) {

	md, ok := metadata.FromIncomingContext(ctx)
	if !ok {
		return nil, status.Errorf(codes.InvalidArgument, "missing metadata")
	}
	token, ok := md["authorization"]
	if !ok || len(token) == 0 {
		return nil, status.Errorf(codes.Unauthenticated, "missing JWT token")
	}

	return handler(context.WithValue(ctx, "token", token[0]), req)
}
func main() {

	db := initDB()

	if db == nil {
		return
	}

	encounterRepo := &repo.EncounterRepository{db}
	encounterService := &service.EncounterService{encounterRepo}
	encounterHandler := &handler.EncounterHandler{EncounterService: encounterService}

	keypointEncRepo := &repo.KeypointEncounterRepository{db}
	keypointEncService := &service.KeypointEncounterService{keypointEncRepo}
	keypointEncHandler := &handler.KeypointEncounterHandler{KeypointEncounterService: keypointEncService}

	completionRepo := &repo.EncounterCompletionRepository{db}

	queueGroup := "encounter_service"
	command := os.Getenv("FINISH_ENCOUNTER_COMMAND_SUBJECT")
	reply := os.Getenv("FINISH_ENCOUNTER_REPLY_SUBJECT")

	commandPublisher := initPublisher(command)
	replySubscriber := initSubscriber(reply, queueGroup)
	orchestrator := initOrchestrator(commandPublisher, replySubscriber)

	completionService := initService(completionRepo, encounterService, orchestrator)

	commandSubscriber := initSubscriber(command, queueGroup)
	replyPublisher := initPublisher(reply)
	initFinishOrderHandler(completionService, replyPublisher, commandSubscriber)

	completionHandler := &handler.EncounterCompletionHandler{EncounterCompletionService: completionService}

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

	grpcServer := grpc.NewServer(
		grpc.UnaryInterceptor(JWTInterceptor))

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
func initSubscriber(subject, queueGroup string) saga.Subscriber {

	subscriber, err := nats.NewSubscriber(subject, queueGroup)
	if err != nil {
		log.Fatalln(err)
	}
	fmt.Printf("Initializing subscriber with subject: %s and queue: %s\n", subject, queueGroup)
	return subscriber
}

func initPublisher(subject string) saga.Publisher {
	publisher, err := nats.NewPublisher(subject)
	if err != nil {
		log.Fatalln(err)
	}
	fmt.Printf("Initializing publisher with subject: %s\n", subject)
	return publisher
}

func initOrchestrator(publisher saga.Publisher, subscriber saga.Subscriber) *service.FinishEncounterOrchestrator {
	orchestrator, err := service.NewFinishOrderOrchestrator(publisher, subscriber)
	if err != nil {
		log.Fatalln(err)
	}
	fmt.Println("Saga orchestrator intialized")
	return orchestrator
}
func initService(repository *repo.EncounterCompletionRepository, encounterService *service.EncounterService, orchestrator *service.FinishEncounterOrchestrator) *service.EncounterCompletionService {
	return service.NewEncounterCompletionService(repository, encounterService, orchestrator)
}
func initFinishOrderHandler(service *service.EncounterCompletionService, publisher saga.Publisher, subscriber saga.Subscriber) {
	_, err := handler.NewFinishEncounterCommandHandler(service, publisher, subscriber)
	if err != nil {
		log.Fatalln(err)
	}
}
