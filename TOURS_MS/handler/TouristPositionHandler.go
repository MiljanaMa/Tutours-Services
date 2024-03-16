package handler

import (
	"encoding/json"
	"log"
	"net/http"
	"tours/model"
	"tours/service"

	"github.com/gorilla/mux"
)

type TouristPositionHandler struct {
	TouristPositionService *service.TouristPositionService
}

func (handler *TouristPositionHandler) GetById(writer http.ResponseWriter, req *http.Request) {
	id := mux.Vars(req)["id"]
	position, err := handler.TouristPositionService.GetById(id)
	writer.Header().Set("Content-Type", "application/json")
	if err != nil {
		writer.WriteHeader(http.StatusNotFound)
		return
	}
	writer.WriteHeader(http.StatusOK)
	json.NewEncoder(writer).Encode(position)
}

func (handler *TouristPositionHandler) GetByUserId(writer http.ResponseWriter, req *http.Request) {
	userId := mux.Vars(req)["id"]
	position, err := handler.TouristPositionService.GetByUserId(userId)
	writer.Header().Set("Content-Type", "application/json")
	if err != nil {
		writer.WriteHeader(http.StatusNotFound)
		return
	}
	writer.WriteHeader(http.StatusOK)
	json.NewEncoder(writer).Encode(position)
}

func (handler *TouristPositionHandler) Create(writer http.ResponseWriter, req *http.Request) {
	var tp model.TouristPosition
	err_decode := json.NewDecoder(req.Body).Decode(&tp)
	if err_decode != nil {
		log.Println(err_decode)
		log.Println("Error while parsing json - create tourist position")
		writer.WriteHeader(http.StatusBadRequest)
		return
	}
	position, err := handler.TouristPositionService.Create(&tp)
	if err != nil {
		log.Println("Error while creating tourist position")
		writer.WriteHeader(http.StatusExpectationFailed)
		return
	}
	writer.WriteHeader(http.StatusCreated)
	writer.Header().Set("Content-Type", "application/json")
	json.NewEncoder(writer).Encode(position)
}

func (handler *TouristPositionHandler) Update(writer http.ResponseWriter, req *http.Request) {
	var tp model.TouristPosition
	err := json.NewDecoder(req.Body).Decode(&tp)
	if err != nil {
		log.Println("Error while parsing json - update tourist position")
		writer.WriteHeader(http.StatusBadRequest)
		return
	}
	position, err := handler.TouristPositionService.Update(&tp)
	if err != nil {
		log.Println("Error while updating position")
		writer.WriteHeader(http.StatusExpectationFailed)
		return
	}
	writer.WriteHeader(http.StatusCreated)
	writer.Header().Set("Content-Type", "application/json")
	json.NewEncoder(writer).Encode(position)
}
