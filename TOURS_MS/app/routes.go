package app

import (
	"tours/handler"

	"github.com/gorilla/mux"
)

func SetupTourRoutes(router *mux.Router, tourHandler *handler.TourHandler) {

	router.HandleFunc("/tours/get/{id}", tourHandler.GetById).Methods("GET")
	router.HandleFunc("/tours/create", tourHandler.Create).Methods("POST")

}

/* TO DO
func SetupKeypointRoutes(router *mux.Router, tourHandler *handler.KeypointHandler) {

}
*/

/* TO DO
func SetupObjectRoutes(router *mux.Router, tourHandler *handler.ObjectHandler) {

}
*/
