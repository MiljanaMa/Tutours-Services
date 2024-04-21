package handler

import (
	"encoding/json"
	"fmt"
	"io"
	"log"
	"net/http"
	"strconv"
	"tours/model"
	"tours/service"

	"github.com/gorilla/mux"
)

type KeypointHandler struct {
	KeypointService *service.KeypointService
}

func (handler *KeypointHandler) GetById(writer http.ResponseWriter, req *http.Request) {
	idStr := mux.Vars(req)["id"]
	id, err := strconv.Atoi(idStr)
	if err != nil {
		http.Error(writer, "Invalid tour ID", http.StatusBadRequest)
		return
	}

	keypoint, err := handler.KeypointService.GetById(id)
	writer.Header().Set("Content-Type", "application/json")
	if err != nil {
		writer.WriteHeader(http.StatusInternalServerError)
		return
	}

	jsonData, err := keypoint.MarshalJSON()
	print(jsonData)
	if err != nil {
		http.Error(writer, "Failed to marshal JSON", http.StatusInternalServerError)
		return
	}

	writer.Header().Set("Content-Type", "application/json")
	writer.Write(jsonData)
}

func (handler *KeypointHandler) GetByTourId(writer http.ResponseWriter, req *http.Request) {
	id := mux.Vars(req)["id"]
	tourId, err := strconv.Atoi(id)

	if err != nil {
		http.Error(writer, "Invalid tour ID", http.StatusBadRequest)
		writer.WriteHeader(http.StatusInternalServerError)
		return
	}

	keypoints, err := handler.KeypointService.GetByTourId(tourId)
	writer.WriteHeader(http.StatusOK)
	keypointJSON := make([]json.RawMessage, len(keypoints))

	for i, keypoint := range keypoints {
		jsonBytes, err := keypoint.MarshalJSON()
		if err != nil {
			http.Error(writer, "Failed to marshal keypoint", http.StatusInternalServerError)
			return
		}
		keypointJSON[i] = json.RawMessage(jsonBytes)
	}

	writer.Header().Set("Content-Type", "application/json")
	if err != nil {
		writer.WriteHeader(http.StatusInternalServerError)
		fmt.Fprintf(writer, `{"error": "Failed to retrieve keypoints for tour with ID %s"}`, id)
		return
	}

	if err := json.NewEncoder(writer).Encode(keypointJSON); err != nil {
		http.Error(writer, "Failed to encode keypoints", http.StatusInternalServerError)
		return
	}
}

func (handler *KeypointHandler) Create(writer http.ResponseWriter, req *http.Request) {
	var k model.Keypoint
	body, err := io.ReadAll(req.Body)

	if err != nil {
		http.Error(writer, "Failed to read request body", http.StatusBadRequest)
		return
	}
	err_decode := k.UnmarshalJSON(body)
	if err_decode != nil {
		log.Println(err_decode)
		log.Println("Error while parsing json - create keypoint")
		writer.WriteHeader(http.StatusBadRequest)
		return
	}
	review, err := handler.KeypointService.Create(&k)
	if err != nil {
		log.Println("Error while creating keypoint")
		http.Error(writer, err.Error(), http.StatusExpectationFailed)
		return
	}
	writer.WriteHeader(http.StatusCreated)
	writer.Header().Set("Content-Type", "application/json")
	json.NewEncoder(writer).Encode(review)
}

func (handler *KeypointHandler) Delete(writer http.ResponseWriter, req *http.Request) {

	idStr := mux.Vars(req)["id"]
	id, err := strconv.Atoi(idStr)
	if err != nil {
		log.Println("Error while parsing query params")
		writer.WriteHeader(http.StatusBadRequest)
		return
	}
	err = handler.KeypointService.Delete(id)

	if err != nil {
		log.Println("Error while deleting keypoint")
		writer.WriteHeader(http.StatusExpectationFailed)
		return
	}

	writer.WriteHeader(http.StatusOK)
}

func (handler *KeypointHandler) Update(writer http.ResponseWriter, req *http.Request) {
	var keypoint model.Keypoint
	body, err := io.ReadAll(req.Body)

	if err != nil {
		log.Println("Error while parsing body")
		http.Error(writer, "Failed to read request body", http.StatusBadRequest)
		return
	}
	err = keypoint.UnmarshalJSON(body)
	if err != nil {
		log.Println("Error while parsing json")
		writer.WriteHeader(http.StatusBadRequest)
		return
	}
	updatedKeypoint, err := handler.KeypointService.Update(&keypoint)
	jsonData, err := updatedKeypoint.MarshalJSON()
	if err != nil {
		log.Println("Error while updating a keypoint")
		writer.WriteHeader(http.StatusExpectationFailed)
		return
	}
	writer.WriteHeader(http.StatusOK)
	writer.Header().Set("Content-Type", "application/json")
	writer.Write(jsonData)
}
