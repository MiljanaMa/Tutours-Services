package handler

import (
	"ENCOUNTERS-MS/model"
	"ENCOUNTERS-MS/service"
	json2 "encoding/json"
	"github.com/gorilla/mux"
	"net/http"
	"strconv"
)

type EncounterHandler struct {
	EncounterService *service.EncounterService
}

func (handler *EncounterHandler) createGetResponse(encounters *[]*model.Encounter, writer http.ResponseWriter) {

	json, err := json2.Marshal(encounters)
	if err != nil {
		writer.WriteHeader(http.StatusInternalServerError)
		writer.Write([]byte("Failed to parse JSON"))
	}

	writer.Header().Set("Content-Type", "application/json")

	writer.WriteHeader(http.StatusOK)
	writer.Write(json)
}

func (handler *EncounterHandler) createPutResponse(writer http.ResponseWriter, req *http.Request, encounter *model.Encounter) {
	res, err := handler.EncounterService.Update(encounter)

	if err != nil {
		writer.WriteHeader(http.StatusInternalServerError)
		writer.Write([]byte("Error updating encounter"))
		return
	}

	json, err := json2.Marshal(res)
	if err != nil {
		writer.WriteHeader(http.StatusInternalServerError)
		writer.Write([]byte("Failed to parse JSON"))
	}

	writer.WriteHeader(http.StatusCreated)
	writer.Header().Set("Content-Type", "application/json")
	writer.Write(json)
}
func (handler *EncounterHandler) GetApproved(writer http.ResponseWriter, req *http.Request) {

	encounters, err := handler.EncounterService.GetApproved()
	if err != nil {
		writer.WriteHeader(http.StatusNotFound)
		return
	}
	handler.createGetResponse(&encounters, writer)

}

func (handler *EncounterHandler) GetTouristCreatedEncounters(writer http.ResponseWriter, req *http.Request) {
	encounters, err := handler.EncounterService.GetTouristCreatedEncounters()
	if err != nil {
		writer.WriteHeader(http.StatusNotFound)
		return

	}
	handler.createGetResponse(&encounters, writer)
}

func (handler *EncounterHandler) GetNearby(writer http.ResponseWriter, req *http.Request) {
	userId, err := strconv.Atoi(mux.Vars(req)["userId"])
	if err != nil {
		writer.WriteHeader(http.StatusBadRequest)
		writer.Write([]byte("Can't parse the user id"))
		return
	}

	encounters, err := handler.EncounterService.GetNearby(userId)
	if err != nil {
		writer.WriteHeader(http.StatusNotFound)

		return
	}
	handler.createGetResponse(&encounters, writer)
}

func (handler *EncounterHandler) GetNearbyByType(writer http.ResponseWriter, req *http.Request) {

	userId, err := strconv.Atoi(mux.Vars(req)["userId"])
	if err != nil {
		writer.WriteHeader(http.StatusBadRequest)
		writer.Write([]byte("Can't parse the user id"))
		return
	}

	encounters, err := handler.EncounterService.GetNearbyHidden(userId)
	if err != nil {
		writer.WriteHeader(http.StatusNotFound)

		return
	}

	handler.createGetResponse(&encounters, writer)

}

func (handler *EncounterHandler) Create(writer http.ResponseWriter, req *http.Request) {
	var encounter model.Encounter

	if err := json2.NewDecoder(req.Body).Decode(&encounter); err != nil {
		writer.WriteHeader(http.StatusBadRequest)
		writer.Write([]byte("Cannot parse JSON"))
		return
	}
	res, err := handler.EncounterService.Create(&encounter)

	if err != nil {
		writer.WriteHeader(http.StatusInternalServerError)
		writer.Write([]byte("Error creating encounter"))
		return
	}

	json, err := json2.Marshal(res)
	if err != nil {
		writer.WriteHeader(http.StatusInternalServerError)
		writer.Write([]byte("Failed to parse JSON"))
	}

	writer.WriteHeader(http.StatusCreated)
	writer.Header().Set("Content-Type", "application/json")
	writer.Write(json)

}

func (handler *EncounterHandler) Delete(writer http.ResponseWriter, req *http.Request) {
	id := mux.Vars(req)["id"]
	numId, err := strconv.Atoi(id)
	if err != nil {
		writer.WriteHeader(http.StatusInternalServerError)
		writer.Write([]byte("Can't convert id"))
		return
	}
	if err := handler.EncounterService.Delete(numId); err != nil {
		writer.WriteHeader(http.StatusNotFound)
		writer.Write([]byte("Can't find id to delete"))
		return
	}

	writer.WriteHeader(http.StatusOK)
	writer.Header().Set("Content-Type", "application/json")

}

func (handler *EncounterHandler) GetByUser(writer http.ResponseWriter, req *http.Request) {
	userId, err := strconv.Atoi(mux.Vars(req)["userId"])
	if err != nil {
		writer.WriteHeader(http.StatusBadRequest)
		writer.Write([]byte("Can't parse the user id"))
		return
	}

	encounters, err := handler.EncounterService.GetByUser(userId)
	if err != nil {
		writer.WriteHeader(http.StatusNotFound)

		return
	}
	handler.createGetResponse(&encounters, writer)

}

func (handler *EncounterHandler) GetApprovedByStatus(writer http.ResponseWriter, req *http.Request) {
	status := mux.Vars(req)["status"]

	encounters, err := handler.EncounterService.GetApprovedByStatus(model.EncounterStatus(status))
	if err != nil {
		writer.WriteHeader(http.StatusNotFound)
		return
	}
	handler.createGetResponse(&encounters, writer)
}

func (handler *EncounterHandler) Approve(writer http.ResponseWriter, req *http.Request) {
	var encounter model.Encounter
	if err := json2.NewDecoder(req.Body).Decode(&encounter); err != nil {
		writer.WriteHeader(http.StatusBadRequest)
		writer.Write([]byte("Cannot parse JSON"))
		return
	}
	encounter.ApprovalStatus = "ADMIN_APPROVED"
	handler.createPutResponse(writer, req, &encounter)
}

func (handler *EncounterHandler) Decline(writer http.ResponseWriter, req *http.Request) {
	var encounter model.Encounter
	if err := json2.NewDecoder(req.Body).Decode(&encounter); err != nil {
		writer.WriteHeader(http.StatusBadRequest)
		writer.Write([]byte("Cannot parse JSON"))
		return
	}
	encounter.ApprovalStatus = "DECLINED"
	handler.createPutResponse(writer, req, &encounter)
}

func (handler *EncounterHandler) Update(writer http.ResponseWriter, req *http.Request) {
	var encounter model.Encounter
	if err := json2.NewDecoder(req.Body).Decode(&encounter); err != nil {
		writer.WriteHeader(http.StatusBadRequest)
		writer.Write([]byte("Cannot parse JSON"))
		return
	}

	handler.createPutResponse(writer, req, &encounter)
}
