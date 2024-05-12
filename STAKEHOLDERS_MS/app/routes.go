package app

import (
	"stakeholder/handler"

	"github.com/gorilla/mux"
)

func SetupStakeholdersRoutes(router *mux.Router, userHandler *handler.UserHandler) {

	router.HandleFunc("/stakeholder/login", userHandler.Login).Methods("POST")
	router.HandleFunc("/stakeholder/validateToken", userHandler.ValidateToken).Methods("POST")
}
