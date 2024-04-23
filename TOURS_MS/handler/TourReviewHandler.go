package handler

import (
	"encoding/json"
	"io"
	"log"
	"net/http"
	"tours/model"
	"tours/service"
)

type TourReviewHandler struct {
	TourReviewService *service.TourReviewService
}

func (handler *TourReviewHandler) GetAll(writer http.ResponseWriter, req *http.Request) {
	reviews, err := handler.TourReviewService.GetAll()
	writer.Header().Set("Content-Type", "application/json")
	if err != nil {
		writer.WriteHeader(http.StatusNotFound)
		return
	}
	writer.WriteHeader(http.StatusOK)
	json.NewEncoder(writer).Encode(reviews)
}

func (handler *TourReviewHandler) Create(writer http.ResponseWriter, req *http.Request) {
	var r model.TourReview
	body, err := io.ReadAll(req.Body)

	if err != nil {
		http.Error(writer, "Failed to read request body", http.StatusBadRequest)
		return
	}
	err_decode := r.UnmarshalJSON(body)
	if err_decode != nil {
		log.Println(err_decode)
		log.Println("Error while parsing json - create tourist review")
		writer.WriteHeader(http.StatusBadRequest)
		return
	}
	review, err := handler.TourReviewService.Create(&r)
	if err != nil {
		log.Println("Error while creating tourist review")
		http.Error(writer, err.Error(), http.StatusExpectationFailed)
		return
	}
	writer.WriteHeader(http.StatusCreated)
	writer.Header().Set("Content-Type", "application/json")
	json.NewEncoder(writer).Encode(review)
}
