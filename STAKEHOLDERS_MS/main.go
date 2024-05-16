package main

import (
	"fmt"
	"google.golang.org/grpc"
	"google.golang.org/grpc/reflection"
	"log"
	"net"
	"os"
	"os/signal"
	"stakeholder/app"
	"stakeholder/handler"
	"stakeholder/proto/stakeholder"
	"stakeholder/repo"
	"stakeholder/service"
	"syscall"
)

func main() {

	app.Init()
	db := app.InitDB()
	app.ExecuteMigrations(db)

	userRepo := &repo.UserRepository{DatabaseConnection: db}
	userService := &service.UserService{UserRepository: userRepo}
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
