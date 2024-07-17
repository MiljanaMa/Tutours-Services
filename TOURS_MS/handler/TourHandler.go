package handler

import (
	"context"
	"google.golang.org/protobuf/types/known/timestamppb"
	"tours/model"
	"tours/model/enum"
	"tours/proto/tour"
	"tours/service"
)

type TourHandler struct {
	tour.UnimplementedTourServiceServer
	TourService *service.TourService
}

func mapTransportType(tourType enum.TransportType) tour.TransportType {
	switch tourType {
	case 0:
		return tour.TransportType_WALK
	case 1:
		return tour.TransportType_BIKE
	case 2:
		return tour.TransportType_CAR
	default:
		return tour.TransportType_BOAT
	}
}
func mapTourStatus(status enum.TourStatus) tour.TourStatus {
	switch status {
	case 0:
		return tour.TourStatus_DRAFT
	case 1:
		return tour.TourStatus_PUBLISHED
	case 2:
		return tour.TourStatus_ARCHIVED
	case 3:
		return tour.TourStatus_DISABLED
	case 4:
		return tour.TourStatus_CUSTOM
	default:
		return tour.TourStatus_CAMPAIGN
	}

}
func mapTourDifficulty(difficulty enum.TourDifficulty) tour.TourDifficulty {
	switch difficulty {
	case 0:
		return tour.TourDifficulty_EASY
	case 1:
		return tour.TourDifficulty_MEDIUM
	case 2:
		return tour.TourDifficulty_HARD
	default:
		return tour.TourDifficulty_EXTREME
	}
}

func inverseTransportType(transportType tour.TransportType) enum.TransportType {
	switch transportType {
	case tour.TransportType_WALK:
		return 0
	case tour.TransportType_BIKE:
		return 1
	case tour.TransportType_CAR:
		return 2
	default:
		return 3
	}
}
func inverseTourStatus(status tour.TourStatus) enum.TourStatus {
	switch status {
	case tour.TourStatus_DRAFT:
		return 0
	case tour.TourStatus_PUBLISHED:
		return 1
	case tour.TourStatus_ARCHIVED:
		return 2
	case tour.TourStatus_DISABLED:
		return 3
	case tour.TourStatus_CUSTOM:
		return 4
	default:
		return 5
	}
}
func inverseTourDifficulty(status tour.TourDifficulty) enum.TourDifficulty {
	switch status {
	case tour.TourDifficulty_EASY:
		return 0
	case tour.TourDifficulty_MEDIUM:
		return 1
	case tour.TourDifficulty_HARD:
		return 2
	default:
		return 3
	}
}

func ModelToRPC(e *model.Tour) *tour.Tour {
	return &tour.Tour{
		Id:             int64(e.Id),
		UserId:         int64(e.UserId),
		Name:           e.Name,
		Description:    e.Description,
		Price:          float32(e.Price),
		Duration:       int64(e.Duration),
		Distance:       float32(e.Distance),
		Status:         mapTourStatus(e.Status),
		TransportType:  mapTransportType(e.TransportType),
		TourDifficulty: mapTourDifficulty(e.Difficulty),
		//Tags:          e.Image,
		StatusUpdateTime: timestamppb.New(e.StatusUpdateTime),
	}
}
func rpcToModel(e *tour.Tour) *model.Tour {
	return &model.Tour{
		Id:            int(e.Id),
		UserId:        int(e.UserId),
		Name:          e.Name,
		Description:   e.Description,
		Price:         float64(e.Price),
		Duration:      int(e.Duration),
		Distance:      float64(e.Distance),
		Status:        inverseTourStatus(e.Status),
		TransportType: inverseTransportType(e.TransportType),
		Difficulty:    inverseTourDifficulty(e.TourDifficulty),
		//Tags:          e.Image,
		//StatusUpdateTime: timestamppb.New(e.StatusUpdateTime),
	}
}

func toRPCtours(tours []model.Tour) *tour.ToursResponse {
	result := make([]*tour.Tour, len(tours))
	for i, e := range tours {
		result[i] = ModelToRPC(&e)
	}
	return &tour.ToursResponse{Tours: result}
}

func (handler *TourHandler) GetAll(ctx context.Context, request *tour.EmptyRequest) (*tour.ToursResponse, error) {

	tours, err := handler.TourService.GetAll(10, 10)
	if err != nil {
		return nil, err
	}
	return toRPCtours(tours), nil

}
func (handler *TourHandler) GetById(ctx context.Context, request *tour.IdRequest) (*tour.Tour, error) {
	id := int(request.Id)

	res, err := handler.TourService.GetById(id)
	if err != nil {
		return nil, err
	}

	return ModelToRPC(&res), nil
}

func (handler *TourHandler) Create(ctx context.Context, request *tour.Tour) (*tour.Tour, error) {
	e := rpcToModel(request)

	res, err := handler.TourService.Create(e)

	if err != nil {
		return nil, err
	}

	return ModelToRPC(&res), nil

}

func (handler *TourHandler) Delete(ctx context.Context, request *tour.IdRequest) (*tour.EmptyResponse, error) {
	id := int(request.Id)

	if err := handler.TourService.Delete(id); err != nil {
		return nil, err
	}

	return &tour.EmptyResponse{}, nil
}

func (handler *TourHandler) GetAllByAuthor(ctx context.Context, request *tour.UserIdRequest) (*tour.ToursResponse, error) {
	userId := int(request.UserId)

	tours, err := handler.TourService.GetAllByAuthorId(10, 10, userId)
	if err != nil {
		return nil, err
	}
	return toRPCtours(tours), nil

}

func (handler *TourHandler) Update(ctx context.Context, request *tour.Tour) (*tour.Tour, error) {
	e := rpcToModel(request)

	_, err := handler.TourService.Update(e)
	if err != nil {
		return nil, err
	}
	return ModelToRPC(e), nil
}

/*
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
}
*/
