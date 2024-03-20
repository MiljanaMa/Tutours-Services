package handler

import (
	"ENCOUNTERS-MS/service"
	"encoding/json"
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
