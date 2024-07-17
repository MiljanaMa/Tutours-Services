package handler

import (
	"context"
	"fmt"
	"stakeholder/model"
	"stakeholder/proto/stakeholder"
	"stakeholder/service"
)

type StakeholderHandler struct {
	stakeholder.UnimplementedStakeholderServiceServer
	UserService *service.UserService
}

func ModelToRPC(e *model.Credentials) *stakeholder.Credentials {
	return &stakeholder.Credentials{
		Username: e.Username,
		Password: e.Password,
	}
}
func rpcToModel(e *stakeholder.Credentials) model.Credentials {
	return model.Credentials{
		Username: e.Username,
		Password: e.Password,
	}
}
func ModelToRPCToken(e *model.AuthenticationTokens) *stakeholder.AuthenticationTokens {
	return &stakeholder.AuthenticationTokens{
		Id:          e.Id,
		AccessToken: e.AccessToken,
	}
}
func rpcToModelToken(e *stakeholder.AuthenticationTokens) *model.AuthenticationTokens {
	return &model.AuthenticationTokens{
		Id:          e.Id,
		AccessToken: e.AccessToken,
	}
}
func (handler *StakeholderHandler) Login(ctx context.Context, request *stakeholder.Credentials) (*stakeholder.AuthenticationTokens, error) {
	fmt.Println("aa" + request.Username)
	token, err := handler.UserService.Login(rpcToModel(request))
	if err != nil {
		return nil, err
	}
	return ModelToRPCToken(&token), nil

}

/*
type UserHandler struct {
	UserService *service.UserService
}

func (handler *UserHandler) Login(writer http.ResponseWriter, req *http.Request) {
	var credentials model.Credentials
	body, err := io.ReadAll(req.Body)

	if err != nil {
		http.Error(writer, "Failed to read request body", http.StatusBadRequest)
		return
	}
	err = json.Unmarshal(body, &credentials)
	if err != nil {
		log.Println("Error while parsing json")
		writer.WriteHeader(http.StatusBadRequest)
		return
	}
	tokens, err := handler.UserService.Login(credentials)
	if err != nil {
		log.Println("Error while creating a new tour")
		http.Error(writer, err.Error(), http.StatusBadRequest)
		return
	}
	jsonData, err := json.Marshal(tokens)
	if err != nil {
		log.Println("Error while parsing a new tour")
		http.Error(writer, err.Error(), http.StatusBadRequest)
		return
	}
	writer.WriteHeader(http.StatusCreated)
	writer.Header().Set("Content-Type", "application/json")
	writer.Write(jsonData)
}
func (handler *UserHandler) ValidateToken(writer http.ResponseWriter, req *http.Request) {
	tokenString := req.Header.Get("Authorization")

	if len(tokenString) > 7 && strings.ToUpper(tokenString[0:7]) == "BEARER " {
		tokenString = tokenString[7:]
	}
	_, err := handler.UserService.ValidateToken(tokenString)
	if err != nil {
		log.Println("Not valid token")
		http.Error(writer, err.Error(), http.StatusUnauthorized)
		return
	}

	writer.Header().Set("Content-Type", "application/json")
	writer.WriteHeader(http.StatusNoContent)
}
*/
