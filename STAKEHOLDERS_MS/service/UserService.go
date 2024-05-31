package service

import (
	"errors"
	"fmt"
	"github.com/dgrijalva/jwt-go"
	"stakeholder/model"
	"stakeholder/repo"
	"stakeholder/util"
)

type UserService struct {
	UserRepository *repo.UserRepository
}

var ErrNotFound = errors.New("not found")
var ErrForbidden = errors.New("forbidden")

func (service *UserService) Login(credentials model.Credentials) (model.AuthenticationTokens, error) {
	var tokens model.AuthenticationTokens
	var user model.User
	user, err := service.UserRepository.GetActiveByUsername(credentials.Username)
	if err != nil {
		return tokens, err
	}
	if credentials.Password != user.Password {
		return tokens, fmt.Errorf("Authentication failed1: %w", ErrNotFound)
	}
	fmt.Println("1" + credentials.Password)
	fmt.Println("2" + user.Password)
	if user.IsBlocked {
		return tokens, fmt.Errorf("Authentication failed2: %w", ErrForbidden)
	}
	/*if !user.IsEnabled {
		return tokens, fmt.Errorf("Authentication failed3: %w", ErrForbidden)
	}
	*/
	var person model.Person
	person, err = service.UserRepository.GetPerson(user.Id)
	if err != nil {
		return tokens, err
	}
	fmt.Println(person.Id)
	jwtGen := util.NewJwtGenerator()
	tokens, err = jwtGen.GenerateAccessToken(&user, person.Id)
	if err != nil {
		fmt.Println("Failed to generate access token:", err)
		return tokens, err
	}
	return tokens, nil
}
func (service *UserService) ValidateToken(token string) (jwt.Claims, error) {
	var claims jwt.MapClaims
	jwtGen := util.NewJwtGenerator()
	claims, err := jwtGen.ValidateAccessToken(token)
	if err != nil {
		fmt.Println("Not valid token:", err)
		return claims, err
	}
	return claims, nil
}

func (service *UserService) AddXp(userId, xp int) error {
	user, err := service.UserRepository.GetPerson(userId)
	if err != nil {
		return err
	}
	if user.XP+xp > 300 {
		return fmt.Errorf("Exceeded XP of 300")
	}
	user.XP += xp
	return service.UserRepository.Update(user)
}
