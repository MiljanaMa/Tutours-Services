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

type TourHandler struct {
	TourService *service.TourService
}

func (handler *TourHandler) GetById(writer http.ResponseWriter, req *http.Request) {
	idStr := mux.Vars(req)["tourId"]
	id, err := strconv.Atoi(idStr)
	if err != nil {
		http.Error(writer, "Invalid user ID", http.StatusBadRequest)
		return
	}

	tour, err := handler.TourService.GetById(id)
	writer.Header().Set("Content-Type", "application/json")
	if err != nil {
		writer.WriteHeader(http.StatusInternalServerError)
		return
	}
	if tour.IsEmpty() {
		writer.WriteHeader(http.StatusNotFound)
		return
	}
	jsonData, err := tour.MarshalJSON()
	print(jsonData)
	if err != nil {
		http.Error(writer, "Failed to marshal JSON", http.StatusInternalServerError)
		return
	}

	writer.Header().Set("Content-Type", "application/json")
	writer.Write(jsonData)
}

func (handler *TourHandler) Create(writer http.ResponseWriter, req *http.Request) {
	var tour model.Tour
	body, err := io.ReadAll(req.Body)

	if err != nil {
		http.Error(writer, "Failed to read request body", http.StatusBadRequest)
		return
	}
	err = tour.UnmarshalJSON(body)
	if err != nil {
		log.Println("Error while parsing json")
		writer.WriteHeader(http.StatusBadRequest)
		return
	}
	savedTour, err := handler.TourService.Create(&tour)
	if err != nil {
		log.Println("Error while creating a new tour")
		http.Error(writer, err.Error(), http.StatusBadRequest)
		return
	}
	jsonData, err := savedTour.MarshalJSON()
	if err != nil {
		log.Println("Error while parsing a new tour")
		http.Error(writer, err.Error(), http.StatusBadRequest)
		return
	}
	writer.WriteHeader(http.StatusCreated)
	writer.Header().Set("Content-Type", "application/json")
	writer.Write(jsonData)
}
func (handler *TourHandler) Update(writer http.ResponseWriter, req *http.Request) {
	var tour model.Tour
	body, err := io.ReadAll(req.Body)

	if err != nil {
		log.Println("Error while parsing body")
		http.Error(writer, "Failed to read request body", http.StatusBadRequest)
		return
	}
	err = tour.UnmarshalJSON(body)
	if err != nil {
		log.Println("Error while parsing json")
		writer.WriteHeader(http.StatusBadRequest)
		return
	}
	updatedTour, err := handler.TourService.Update(&tour)
	jsonData, err := updatedTour.MarshalJSON()
	if err != nil {
		log.Println("Error while updating a tour")
		writer.WriteHeader(http.StatusExpectationFailed)
		return
	}
	writer.WriteHeader(http.StatusOK)
	writer.Header().Set("Content-Type", "application/json")
	writer.Write(jsonData)
}
func (handler *TourHandler) Delete(writer http.ResponseWriter, req *http.Request) {

	idStr := mux.Vars(req)["tourId"]
	tourId, err := strconv.Atoi(idStr)
	if err != nil {
		log.Println("Error while parsing query params")
		writer.WriteHeader(http.StatusBadRequest)
		return
	}
	err = handler.TourService.Delete(tourId)

	if err != nil {
		log.Println("Error while updating tour key point")
		writer.WriteHeader(http.StatusExpectationFailed)
		return
	}

	writer.WriteHeader(http.StatusOK)
}
func (handler *TourHandler) GetAll(writer http.ResponseWriter, req *http.Request) {
	pageStr := req.URL.Query().Get("page")
	limitStr := req.URL.Query().Get("pageSize")

	limit, err := strconv.Atoi(limitStr)
	if err != nil {
		http.Error(writer, "Failed to read page size", http.StatusInternalServerError)
	}
	page, err := strconv.Atoi(pageStr)
	if err != nil {
		http.Error(writer, "Failed to read page numbers", http.StatusInternalServerError)
		writer.WriteHeader(http.StatusInternalServerError)
		return
	}

	tours, err := handler.TourService.GetAll(limit, page)
	if err != nil {
		http.Error(writer, "Failed to fetch tours", http.StatusInternalServerError)
		return
	}
	writer.WriteHeader(http.StatusOK)
	json.NewEncoder(writer).Encode(tours)
}

func (handler *TourHandler) GetAllByAuthor(writer http.ResponseWriter, req *http.Request) {
	fmt.Println("Handler: GetAllByAuthor called")

	idStr := mux.Vars(req)["authorId"]
	authorId, err := strconv.Atoi(idStr)
	if err != nil {
		http.Error(writer, "Invalid user ID", http.StatusBadRequest)
		writer.WriteHeader(http.StatusInternalServerError)
		return
	}
	//zapucano dok ne skontam zasto ne radi kao query param
	limit := 10
	page := 1
	tours, err := handler.TourService.GetAllByAuthorId(limit, page, authorId)

	writer.WriteHeader(http.StatusOK)

	// Create a slice to hold the marshaled tour JSON strings
	tourJSON := make([]json.RawMessage, len(tours))

	// Marshal each tour individually
	for i, tour := range tours {
		jsonBytes, err := tour.MarshalJSON()
		if err != nil {
			http.Error(writer, "Failed to marshal tour", http.StatusInternalServerError)
			return
		}
		tourJSON[i] = json.RawMessage(jsonBytes)
	}

	// Encode the marshaled tour JSON strings
	if err := json.NewEncoder(writer).Encode(tourJSON); err != nil {
		http.Error(writer, "Failed to encode tours", http.StatusInternalServerError)
		return
	}
	/*
		if err != nil {
			http.Error(writer, "Failed to fetch tours", http.StatusInternalServerError)
			return
		}
		writer.WriteHeader(http.StatusOK)
		json.NewEncoder(writer).Encode(tours)*/
}
