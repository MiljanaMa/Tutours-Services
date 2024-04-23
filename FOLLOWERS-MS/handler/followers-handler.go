package handler

import (
	"FOLLOWERS-MS/service"
	"encoding/json"
	"fmt"
	"net/http"
	"strconv"

	"github.com/gorilla/mux"
)

type FollowersHandler struct {
	FollowerService *service.FollowerService
}

func (handler *FollowersHandler) Follow(writer http.ResponseWriter, req *http.Request) {
	vars := mux.Vars(req)
	id1, err1 := strconv.Atoi(vars["id1"])
	id2, err2 := strconv.Atoi(vars["id2"])
	if err1 != nil || err2 != nil {
		fmt.Println("Ids must be integers")
		writer.WriteHeader(http.StatusBadRequest)
		return
	}
	var err = handler.FollowerService.Follow(id1, id2)
	if err == nil {
		fmt.Println("Successfully added connection between users")
		writer.WriteHeader(http.StatusOK)
		return
	}
	fmt.Println(err)
	fmt.Println("Error creating connection between users")
	writer.WriteHeader(http.StatusInternalServerError)

}

func (handler *FollowersHandler) CheckIfFollowing(writer http.ResponseWriter, req *http.Request) {
	vars := mux.Vars(req)
	id1, err1 := strconv.Atoi(vars["id1"])
	id2, err2 := strconv.Atoi(vars["id2"])
	if err1 != nil || err2 != nil {
		fmt.Println("Ids must be integers")
		writer.WriteHeader(http.StatusBadRequest)
		return
	}

	result, err := handler.FollowerService.IsFollowing(id1, id2)
	if err != nil {
		fmt.Println("Error checking")
		writer.WriteHeader(http.StatusInternalServerError)
		return
	}

	response := []byte(strconv.FormatBool(result))
	writer.WriteHeader(http.StatusOK)
	writer.Write(response)
}

func (handler *FollowersHandler) GetRecommendation(writer http.ResponseWriter, req *http.Request) {
	vars := mux.Vars(req)
	id, err := strconv.Atoi(vars["id"])

	fmt.Println("DA")
	if err != nil {
		fmt.Println("Id must be integer")
		writer.WriteHeader(http.StatusBadRequest)
		return
	}

	result, error := handler.FollowerService.GetRecommendations(id)

	if error != nil {
		fmt.Println(error)
		writer.WriteHeader(http.StatusInternalServerError)
		return
	}

	response, err := json.Marshal(result)
	if err != nil {
		fmt.Println(err)
		writer.WriteHeader(http.StatusInternalServerError)
		return
	}
	writer.Header().Set("Content-Type", "application/json")
	writer.WriteHeader(http.StatusOK)
	writer.Write(response)
}

func (handler *FollowersHandler) Unfollow(writer http.ResponseWriter, req *http.Request) {
	vars := mux.Vars(req)
	id1, err1 := strconv.Atoi(vars["id1"])
	id2, err2 := strconv.Atoi(vars["id2"])
	if err1 != nil || err2 != nil {
		fmt.Println("Ids must be integers")
		writer.WriteHeader(http.StatusBadRequest)
		return
	}
	var err = handler.FollowerService.Unfollow(id1, id2)
	if err == nil {
		fmt.Println("Successfully deleted connection between users")
		writer.WriteHeader(http.StatusOK)
		return
	}
	fmt.Println(err)
	fmt.Println("Error deleting connection between users")
	writer.WriteHeader(http.StatusInternalServerError)

}

func (handler *FollowersHandler) GetFollowings(writer http.ResponseWriter, req *http.Request) {
	vars := mux.Vars(req)
	id, err := strconv.Atoi(vars["id"])

	if err != nil {
		fmt.Println("Id must be integer")
		writer.WriteHeader(http.StatusBadRequest)
		return
	}

	result, error := handler.FollowerService.GetFollowings(id)

	if error != nil {
		fmt.Println(error)
		writer.WriteHeader(http.StatusInternalServerError)
		return
	}

	response, err := json.Marshal(result)
	if err != nil {
		fmt.Println(err)
		writer.WriteHeader(http.StatusInternalServerError)
		return
	}
	writer.Header().Set("Content-Type", "application/json")
	writer.WriteHeader(http.StatusOK)
	writer.Write(response)
}

func (handler *FollowersHandler) GetFollowers(writer http.ResponseWriter, req *http.Request) {
	vars := mux.Vars(req)
	id, err := strconv.Atoi(vars["id"])

	if err != nil {
		fmt.Println("Id must be integer")
		writer.WriteHeader(http.StatusBadRequest)
		return
	}

	result, error := handler.FollowerService.GetFollowers(id)

	if error != nil {
		fmt.Println(error)
		writer.WriteHeader(http.StatusInternalServerError)
		return
	}

	response, err := json.Marshal(result)
	if err != nil {
		fmt.Println(err)
		writer.WriteHeader(http.StatusInternalServerError)
		return
	}
	writer.Header().Set("Content-Type", "application/json")
	writer.WriteHeader(http.StatusOK)
	writer.Write(response)
}
