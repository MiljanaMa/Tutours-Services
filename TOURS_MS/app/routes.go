package app

import (
	"tours/handler"

	"github.com/gorilla/mux"
)

func SetupTourRoutes(router *mux.Router, tourHandler *handler.TourHandler) {

	router.HandleFunc("/tours/get/{id}", tourHandler.GetById).Methods("GET")
	router.HandleFunc("/tours/create", tourHandler.Create).Methods("POST")

}

func SetupKeypointRoutes(router *mux.Router, keypointHandler *handler.KeypointHandler) {

	router.HandleFunc("/keypoints/get/{id}", keypointHandler.GetById).Methods("GET")
	router.HandleFunc("/keypoints/getByTour/{id}", keypointHandler.GetByTourId).Methods("GET")
	router.HandleFunc("/keypoints/create", keypointHandler.Create).Methods("POST")
	router.HandleFunc("/keypoints/update", keypointHandler.Update).Methods("POST")
	router.HandleFunc("/keypoints/delete/{id}", keypointHandler.Delete).Methods("DELETE")

}

func SetupTouristPositionRoutes(router *mux.Router, touristPositionHandler *handler.TouristPositionHandler) {

	router.HandleFunc("/positions/get/{id}", touristPositionHandler.GetById).Methods("GET")
	router.HandleFunc("/positions/getByUser/{id}", touristPositionHandler.GetByUserId).Methods("GET")
	router.HandleFunc("/positions/create", touristPositionHandler.Create).Methods("POST")
	router.HandleFunc("/positions/update", touristPositionHandler.Update).Methods("POST")
}
