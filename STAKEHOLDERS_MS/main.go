package main

import (
	"fmt"
	"log"
	"net"
	"os"
	"os/signal"
	"stakeholder/app"
	"stakeholder/handler"
	"stakeholder/proto/stakeholder"
	"stakeholder/repo"
	saga "stakeholder/saga/messaging"
	"stakeholder/saga/messaging/nats"
	"stakeholder/service"
	"syscall"

	"google.golang.org/grpc"
	"google.golang.org/grpc/reflection"
)

func main() {

	app.Init()
	db := app.InitDB()
	app.ExecuteMigrations(db)

	userRepo := &repo.UserRepository{DatabaseConnection: db}
	userService := &service.UserService{UserRepository: userRepo}

	queueGroup := "stakeholder_service"
	command := os.Getenv("FINISH_ENCOUNTER_COMMAND_SUBJECT")
	reply := os.Getenv("FINISH_ENCOUNTER_REPLY_SUBJECT")

	commandSubscriber := initSubscriber(command, queueGroup)
	replyPublisher := initPublisher(reply)
	initFinishOrderHandler(userService, replyPublisher, commandSubscriber)

	userHandler := &handler.StakeholderHandler{UserService: userService}

	lis, err := net.Listen("tcp", ":8099")
	fmt.Println("Running gRPC on port 8099")
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

	stakeholder.RegisterStakeholderServiceServer(grpcServer, userHandler)

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
	/*
		router := mux.NewRouter()

		app.SetupStakeholdersRoutes(router, userHandler)

		log.Fatal(http.ListenAndServe(app.Port, router))*/
}

func initSubscriber(subject, queueGroup string) saga.Subscriber {
	subscriber, err := nats.NewSubscriber(subject, queueGroup)
	if err != nil {
		log.Fatalln(err)
	}
	return subscriber
}

func initPublisher(subject string) saga.Publisher {
	publisher, err := nats.NewPublisher(subject)
	if err != nil {
		log.Fatalln(err)
	}
	return publisher
}

func initFinishOrderHandler(service *service.UserService, publisher saga.Publisher, subscriber saga.Subscriber) {
	_, err := handler.NewFinishEncounterCommandHandler(service, publisher, subscriber)
	if err != nil {
		log.Fatalln(err)
	}
}
