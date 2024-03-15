package app

import (
	"tours/handler"

	"github.com/gorilla/mux"
)

func SetupTourRoutes(router *mux.Router, tourHandler *handler.TourHandler) {

	router.HandleFunc("/tours/{tourId}", tourHandler.GetById).Methods("GET")
	router.HandleFunc("/tours/create", tourHandler.Create).Methods("POST")
	router.HandleFunc("/tours/", tourHandler.GetAll).Methods("GET")
	router.HandleFunc("/tours/author", tourHandler.GetAllByAuthor).Methods("GET")
}

/* TO DO
func SetupKeypointRoutes(router *mux.Router, tourHandler *handler.KeypointHandler) {

}
*/

/* TO DO
func SetupObjectRoutes(router *mux.Router, tourHandler *handler.ObjectHandler) {

}
*/
