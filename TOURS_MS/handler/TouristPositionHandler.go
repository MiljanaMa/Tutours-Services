package handler

import (
	"encoding/json"
	"io"
	"log"
	"net/http"
	"strconv"
	"tours/model"
	"tours/service"

	"github.com/gorilla/mux"
)

type TouristPositionHandler struct {
	TouristPositionService *service.TouristPositionService
}

func (handler *TouristPositionHandler) GetById(writer http.ResponseWriter, req *http.Request) {
	idStr := mux.Vars(req)["id"]
	id, err := strconv.Atoi(idStr)
	if err != nil {
		http.Error(writer, "Invalid position ID", http.StatusBadRequest)
		return
	}

	position, err := handler.TouristPositionService.GetById(id)
	writer.Header().Set("Content-Type", "application/json")
	if err != nil {
		writer.WriteHeader(http.StatusInternalServerError)
		return
	}

	jsonData, err := position.MarshalJSON()
	print(jsonData)
	if err != nil {
		http.Error(writer, "Failed to marshal JSON", http.StatusInternalServerError)
		return
	}

	writer.Header().Set("Content-Type", "application/json")
	writer.Write(jsonData)
}

func (handler *TouristPositionHandler) GetByUserId(writer http.ResponseWriter, req *http.Request) {
	idStr := mux.Vars(req)["id"]
	id, err := strconv.Atoi(idStr)
	if err != nil {
		http.Error(writer, "Invalid user ID", http.StatusBadRequest)
		return
	}

	position, err := handler.TouristPositionService.GetByUserId(id)
	writer.Header().Set("Content-Type", "application/json")
	if err != nil {
		writer.WriteHeader(http.StatusInternalServerError)
		return
	}

	jsonData, err := position.MarshalJSON()
	print(jsonData)
	if err != nil {
		http.Error(writer, "Failed to marshal JSON", http.StatusInternalServerError)
		return
	}

	writer.Header().Set("Content-Type", "application/json")
	writer.Write(jsonData)
}

func (handler *TouristPositionHandler) Create(writer http.ResponseWriter, req *http.Request) {
	var tp model.TouristPosition
	body, err := io.ReadAll(req.Body)

	if err != nil {
		http.Error(writer, "Failed to read request body", http.StatusBadRequest)
		return
	}
	err_decode := tp.UnmarshalJSON(body)
	if err_decode != nil {
		log.Println(err_decode)
		log.Println("Error while parsing json - create position")
		writer.WriteHeader(http.StatusBadRequest)
		return
	}
	position, err := handler.TouristPositionService.Create(&tp)
	if err != nil {
		log.Println("Error while creating position")
		http.Error(writer, err.Error(), http.StatusExpectationFailed)
		return
	}
	writer.WriteHeader(http.StatusCreated)
	writer.Header().Set("Content-Type", "application/json")
	json.NewEncoder(writer).Encode(position)
}

func (handler *TouristPositionHandler) Update(writer http.ResponseWriter, req *http.Request) {
	var tp model.TouristPosition
	body, err := io.ReadAll(req.Body)

	if err != nil {
		log.Println("Error while parsing body")
		http.Error(writer, "Failed to read request body", http.StatusBadRequest)
		return
	}
	err = tp.UnmarshalJSON(body)
	if err != nil {
		log.Println("Error while parsing json")
		writer.WriteHeader(http.StatusBadRequest)
		return
	}
	updatedPosition, err := handler.TouristPositionService.Update(&tp)
	if err != nil {
		log.Println("Error while updating a position")
		writer.WriteHeader(http.StatusExpectationFailed)
		return
	}

	jsonData, err := updatedPosition.MarshalJSON()
	if err != nil {
		log.Println("Error while marshaling a position")
		writer.WriteHeader(http.StatusExpectationFailed)
		return
	}
	writer.WriteHeader(http.StatusOK)
	writer.Header().Set("Content-Type", "application/json")
	writer.Write(jsonData)
}
