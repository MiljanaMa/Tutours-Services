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
		if len(keypoints) == 0 {
			// No keypoints found error
			writer.WriteHeader(http.StatusNotFound)
			fmt.Fprintf(writer, `{"error": "No keypoints found for tour with ID %s"}`, id)
		} else {
			// Other errors
			writer.WriteHeader(http.StatusInternalServerError)
			fmt.Fprintf(writer, `{"error": "Failed to retrieve keypoints for tour with ID %s"}`, id)
		}
		return
	}

	// Write the successful response with keypoints data
	writer.WriteHeader(http.StatusOK)
	json.NewEncoder(writer).Encode(keypoints)
}

func (handler *KeypointHandler) Create(writer http.ResponseWriter, req *http.Request) {
	var kp model.Keypoint
	err := json.NewDecoder(req.Body).Decode(&kp)
	if err != nil {
		log.Println("Error while parsing json")
		writer.WriteHeader(http.StatusBadRequest)
		return
	}
	keypoint, err := handler.KeypointService.Create(&kp)
	if err != nil {
		log.Println("Error while creating a new keypoint")
		writer.WriteHeader(http.StatusExpectationFailed)
		return
	}
	writer.WriteHeader(http.StatusCreated)
	writer.Header().Set("Content-Type", "application/json")
	json.NewEncoder(writer).Encode(keypoint)
}
