package main

import (
	"fmt"
	"log"
	"net"
	"os"
	"os/signal"
	"syscall"
	"tours/app"
	"tours/handler"
	"tours/proto/tour"
	"tours/repo"
	"tours/service"

	"google.golang.org/grpc"
	"google.golang.org/grpc/reflection"
)

func main() {

	app.Init()
	client := app.InitDB()
	storeLogger := log.New(os.Stdout, "[patient-store] ", log.LstdFlags)
	app.InsertInfo(client)
	// Tours setup
	tourRepo := &repo.TourRepository{Cli: client, Logger: storeLogger}
	tourService := &service.TourService{TourRepository: tourRepo}
	tourHandler := &handler.TourHandler{TourService: tourService}

	touristPositionRepo := &repo.TouristPositionRepository{Cli: client, Logger: storeLogger}
	touristPositionService := &service.TouristPositionService{TouristPositionRepository: touristPositionRepo}
	touristPositionHandler := &handler.TouristPositionHandler{TouristPositionService: touristPositionService}

	lis, err := net.Listen("tcp", ":8000")
	fmt.Println("Running gRPC on port 8000")
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

	tour.RegisterTourServiceServer(grpcServer, tourHandler)
	tour.RegisterTouristPositionServiceServer(grpcServer, touristPositionHandler)

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
		// Keypoints setup
		keypointRepo := &repo.KeypointRepository{Cli: client, Logger: storeLogger}
		keypointService := &service.KeypointService{KeypointRepository: keypointRepo}
		keypointHandler := &handler.KeypointHandler{KeypointService: keypointService}

		// Tourist review setup
		tourReviewRepo := &repo.TourReviewRepository{Cli: client, Logger: storeLogger}
		tourReviewService := &service.TourReviewService{TourReviewRepository: tourReviewRepo}
		tourReviewHandler := &handler.TourReviewHandler{TourReviewService: tourReviewService}

		router := mux.NewRouter()

		app.SetupTourRoutes(router, tourHandler)
		app.SetupKeypointRoutes(router, keypointHandler)
		app.SetupTouristPositionRoutes(router, touristPositionHandler)
		app.SetupTourReviewRoutes(router, tourReviewHandler)

		log.Fatal(http.ListenAndServe(app.Port, router))*/
}
