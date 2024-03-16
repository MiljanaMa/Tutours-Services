package handler

import (
	"encoding/json"
	"fmt"
	"log"
	"net/http"
	"tours/model"
	"tours/service"

	"github.com/gorilla/mux"
)

type KeypointHandler struct {
	KeypointService *service.KeypointService
}

func (handler *KeypointHandler) GetById(writer http.ResponseWriter, req *http.Request) {
	id := mux.Vars(req)["id"]
	keypoint, err := handler.KeypointService.GetById(id)
	writer.Header().Set("Content-Type", "application/json")
	if err != nil {
		writer.WriteHeader(http.StatusNotFound)
		return
	}
	writer.WriteHeader(http.StatusOK)
	json.NewEncoder(writer).Encode(keypoint)
}

func (handler *KeypointHandler) GetByTourId(writer http.ResponseWriter, req *http.Request) {
	id := mux.Vars(req)["id"]
	keypoints, err := handler.KeypointService.GetByTourId(id)
	writer.Header().Set("Content-Type", "application/json")
	if err != nil {
		writer.WriteHeader(http.StatusInternalServerError)
		fmt.Fprintf(writer, `{"error": "Failed to retrieve keypoints for tour with ID %s"}`, id)
		return
	}

	writer.WriteHeader(http.StatusOK)
	json.NewEncoder(writer).Encode(keypoints)
}

func (handler *KeypointHandler) Create(writer http.ResponseWriter, req *http.Request) {
	var kp model.Keypoint
	err_decode := json.NewDecoder(req.Body).Decode(&kp)
	if err_decode != nil {
		log.Println("Error while parsing json")
		writer.WriteHeader(http.StatusBadRequest)
		return
	}
	keypoint, err := handler.KeypointService.Create(&kp)
	if err != nil {
		log.Println("Error while creating keypoint")
		writer.WriteHeader(http.StatusExpectationFailed)
		return
	}
	writer.WriteHeader(http.StatusCreated)
	writer.Header().Set("Content-Type", "application/json")
	json.NewEncoder(writer).Encode(keypoint)
}

func (handler *KeypointHandler) Update(writer http.ResponseWriter, req *http.Request) {
	var kp model.Keypoint
	err := json.NewDecoder(req.Body).Decode(&kp)
	if err != nil {
		log.Println("Error while parsing json")
		writer.WriteHeader(http.StatusBadRequest)
		return
	}
	keypoint, err := handler.KeypointService.Update(&kp)
	if err != nil {
		log.Println("Error while updating a new keypoint")
		writer.WriteHeader(http.StatusExpectationFailed)
		return
	}
	writer.WriteHeader(http.StatusCreated)
	writer.Header().Set("Content-Type", "application/json")
	json.NewEncoder(writer).Encode(keypoint)
}

func (handler *KeypointHandler) Delete(writer http.ResponseWriter, req *http.Request) {
	id := mux.Vars(req)["id"]
	err := handler.KeypointService.Delete(id)
	writer.Header().Set("Content-Type", "application/json")
	if err != nil {
		writer.WriteHeader(http.StatusInternalServerError)
		fmt.Fprintf(writer, `{"error": "Failed to delete keypoint}`)
	}
	writer.WriteHeader(http.StatusOK)
}
