package app

import (
	"tours/handler"

	"github.com/gorilla/mux"
)

func SetupTourRoutes(router *mux.Router, tourHandler *handler.TourHandler) {

	router.HandleFunc("/tours/get/{id}", tourHandler.GetById).Methods("GET")
	router.HandleFunc("/tours/create", tourHandler.Create).Methods("POST")

}

func SetupKeypointRoutes(router *mux.Router, tourHandler *handler.KeypointHandler) {

	router.HandleFunc("/keypoints/get/{id}", tourHandler.GetById).Methods("GET")
	router.HandleFunc("/keypoints/getByTour/{id}", tourHandler.GetByTourId).Methods("GET")
	router.HandleFunc("/keypoints/create", tourHandler.Create).Methods("POST")

}

/* TO DO
func SetupObjectRoutes(router *mux.Router, tourHandler *handler.ObjectHandler) {

}
*/
