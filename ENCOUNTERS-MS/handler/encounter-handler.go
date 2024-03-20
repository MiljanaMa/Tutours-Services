package handler

import (
	"ENCOUNTERS-MS/service"
	json2 "encoding/json"
	"net/http"
)

type EncounterHandler struct {
	EncounterService *service.EncounterService
}

func (handler *EncounterHandler) GetApproved(writer http.ResponseWriter, req *http.Request) {

	encounters, err := handler.EncounterService.GetApproved()

	if err != nil {
		writer.WriteHeader(http.StatusInternalServerError)
		writer.Write([]byte("Can't find any approved encounters"))
		return
	}

	json, err := json2.Marshal(encounters)
	if err != nil {
		writer.WriteHeader(http.StatusInternalServerError)
		writer.Write([]byte("Failed to parse JSON"))
	}
	writer.Header().Set("Content-Type", "application/json")

	writer.WriteHeader(http.StatusOK)
	writer.Write(json)
}
