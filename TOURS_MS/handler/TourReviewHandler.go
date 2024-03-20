package handler

import (
	"encoding/json"
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
	err_decode := json.NewDecoder(req.Body).Decode(&r)
	if err_decode != nil {
		log.Println(err_decode)
		log.Println("Error while parsing json - create tourist review")
		writer.WriteHeader(http.StatusBadRequest)
		return
	}
	review, err := handler.TourReviewService.Create(&r)
	if err != nil {
		log.Println("Error while creating tourist review")
		writer.WriteHeader(http.StatusExpectationFailed)
		return
	}
	writer.WriteHeader(http.StatusCreated)
	writer.Header().Set("Content-Type", "application/json")
	json.NewEncoder(writer).Encode(review)
}
