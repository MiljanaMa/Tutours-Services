package main

import (
	"github.com/gorilla/mux"
	"log"
	"net/http"
	"stakeholder/app"
	"stakeholder/handler"
	"stakeholder/repo"
	"stakeholder/service"
)

func main() {

	app.Init()
	db := app.InitDB()
	app.ExecuteMigrations(db)

	userRepo := &repo.UserRepository{DatabaseConnection: db}
	userService := &service.UserService{UserRepository: userRepo}
	userHandler := &handler.UserHandler{UserService: userService}

	router := mux.NewRouter()

	app.SetupStakeholdersRoutes(router, userHandler)

	log.Fatal(http.ListenAndServe(app.Port, router))
}
