package handler

import (
	"ENCOUNTERS-MS/model"
	"ENCOUNTERS-MS/service"
	"encoding/json"
	"fmt"
	"net/http"

	"github.com/gorilla/mux"
)

type KeypointEncounterHandler struct {
	KeypointEncounterService *service.KeypointEncounterService
}

func (handler *KeypointEncounterHandler) GetPagedByKeypoint(writer http.ResponseWriter, req *http.Request) {
	fmt.Println("test 1")
	keypointId := mux.Vars(req)["keypointid"]

	fmt.Println("test 2")

	result, err := handler.KeypointEncounterService.GetPagedByKeypoint(keypointId)
	if err != nil {
		writer.WriteHeader(http.StatusInternalServerError)
		writer.Write([]byte("Error fetching paged results"))
		return
	}

	jsonData, err := json.Marshal(result)
	if err != nil {
		writer.WriteHeader(http.StatusInternalServerError)
		writer.Write([]byte("Failed to parse JSON"))
		return
	}

	writer.Header().Set("Content-Type", "application/json")
	writer.WriteHeader(http.StatusOK)
	writer.Write(jsonData)
}

func (handler *KeypointEncounterHandler) Create(writer http.ResponseWriter, req *http.Request) {
	fmt.Println("test create 1")

	var keypointEncounter model.KeypointEncounter
	err := json.NewDecoder(req.Body).Decode(&keypointEncounter)
	if err != nil {
		writer.WriteHeader(http.StatusBadRequest)
		writer.Write([]byte("Invalid request body"))
		return
	}
	fmt.Println("test create 2")

	//prebaci vars := mux.Vars(req)
	//userId := vars["id"]
	//keypointEncounter.Encounter.UserId = userId prebaci

	result, err := handler.KeypointEncounterService.Create(&keypointEncounter)
	if err != nil {
		writer.WriteHeader(http.StatusInternalServerError)
		writer.Write([]byte(fmt.Sprintf("Error creating keypoint encounter: %s", err)))
		return
	}

	jsonData, err := json.Marshal(result)
	if err != nil {
		writer.WriteHeader(http.StatusInternalServerError)
		writer.Write([]byte("Failed to parse JSON"))
		return
	}

	writer.Header().Set("Content-Type", "application/json")
	writer.WriteHeader(http.StatusOK)
	writer.Write(jsonData)
}

func (handler *KeypointEncounterHandler) Update(writer http.ResponseWriter, req *http.Request) {
	var keypointEncounter model.KeypointEncounter
	err := json.NewDecoder(req.Body).Decode(&keypointEncounter)
	if err != nil {
		writer.WriteHeader(http.StatusBadRequest)
		writer.Write([]byte("Invalid request body"))
		return
	}

	result, err := handler.KeypointEncounterService.Update(&keypointEncounter)
	if err != nil {
		writer.WriteHeader(http.StatusInternalServerError)
		writer.Write([]byte(fmt.Sprintf("Error updating keypoint encounter: %s", err)))
		return
	}

	jsonData, err := json.Marshal(result)
	if err != nil {
		writer.WriteHeader(http.StatusInternalServerError)
		writer.Write([]byte("Failed to parse JSON"))
		return
	}

	writer.Header().Set("Content-Type", "application/json")
	writer.WriteHeader(http.StatusOK)
	writer.Write(jsonData)
}

func (handler *KeypointEncounterHandler) Delete(writer http.ResponseWriter, req *http.Request) {
	id := req.URL.Query().Get("id")

	err := handler.KeypointEncounterService.Delete(id)
	if err != nil {
		writer.WriteHeader(http.StatusInternalServerError)
		writer.Write([]byte(fmt.Sprintf("Error deleting keypoint encounter: %s", err)))
		return
	}

	writer.WriteHeader(http.StatusNoContent)
}
