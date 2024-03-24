package handler

import (
	"ENCOUNTERS-MS/service"
	"encoding/json"
	"fmt"
	"net/http"

	"github.com/gorilla/mux"
)

type EncounterCompletionHandler struct {
	EncounterCompletionService *service.EncounterCompletionService
}

func (handler *EncounterCompletionHandler) GetPagedByUser(writer http.ResponseWriter, req *http.Request) {
	userId := mux.Vars(req)["id"]

	encounterCompletions, err := handler.EncounterCompletionService.GetPagedByUser(userId)

	if err != nil {
		writer.WriteHeader(http.StatusInternalServerError)
		writer.Write([]byte("Can't find any encounter completions"))
		return
	}

	jsonData, err := json.Marshal(encounterCompletions)
	if err != nil {
		writer.WriteHeader(http.StatusInternalServerError)
		writer.Write([]byte("Failed to parse JSON"))
		return
	}

	writer.Header().Set("Content-Type", "application/json")
	writer.WriteHeader(http.StatusOK)
	writer.Write(jsonData)
}

func (handler *EncounterCompletionHandler) FinishEncounter(writer http.ResponseWriter, req *http.Request) {
	fmt.Println("aaaaaaaaaaaaaaa")

	encounterId := "f47ac10b-58cc-4372-a567-0e02b2c3d479" //mora se menjati na frontu sve ili ovde da je id tipa int a ne uuid
	vars := mux.Vars(req)
	userId := vars["id"]

	result, err := handler.EncounterCompletionService.FinishEncounter(userId, encounterId)
	if err != nil {
		writer.WriteHeader(http.StatusInternalServerError)
		writer.Write([]byte(fmt.Sprintf("Error finishing encounter: %s", err)))
		return
	}

	jsonData, err := json.Marshal(result)
	if err != nil {
		writer.WriteHeader(http.StatusInternalServerError)
		writer.Write([]byte("Failed to parse JSON"))
		return
	}
	fmt.Println(string(jsonData))

	writer.Header().Set("Content-Type", "application/json")
	writer.WriteHeader(http.StatusOK)
	writer.Write(jsonData)
}
