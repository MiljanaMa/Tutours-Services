package util

import (
	"fmt"
	"github.com/dgrijalva/jwt-go"
	"github.com/google/uuid"
	"os"
	"stakeholder/model"
	"strconv"
)

type JwtGenerator struct {
	Audience string
	Issuer   string
	Key      string
}

func NewJwtGenerator() *JwtGenerator {
	return &JwtGenerator{
		Audience: getEnv("JWT_AUDIENCE", "explorer-front.com"),
		Issuer:   getEnv("JWT_ISSUER", "explorer"),
		Key:      getEnv("JWT_KEY", "explorer_secret_key"),
	}
}

func (jwtGen *JwtGenerator) GenerateAccessToken(user *model.User, personID int) (model.AuthenticationTokens, error) {
	var authToken model.AuthenticationTokens

	claims := jwt.MapClaims{
		"jti":      uuid.New(),
		"id":       user.Id,
		"username": user.Username,
		"personId": personID,
		"role":     user.Role,
	}

	token := jwt.NewWithClaims(jwt.SigningMethodHS256, claims)
	jwtString, err := token.SignedString([]byte(jwtGen.Key))
	if err != nil {
		return authToken, fmt.Errorf("failed to generate access token: %v", err)
	}

	authToken.Id = strconv.Itoa(user.Id)
	authToken.AccessToken = jwtString

	return authToken, nil
}
func (jwtGen *JwtGenerator) ValidateAccessToken(tokenString string) (jwt.MapClaims, error) {
	token, err := jwt.Parse(tokenString, func(token *jwt.Token) (interface{}, error) {
		if _, ok := token.Method.(*jwt.SigningMethodHMAC); !ok {
			return nil, fmt.Errorf("unexpected signing method: %v", token.Header["alg"])
		}
		// Return the key for validation
		return []byte(jwtGen.Key), nil
	})
	if err != nil {
		return nil, fmt.Errorf("token parsing error: %v", err)
	}

	// Check if the token is valid
	if !token.Valid {
		return nil, fmt.Errorf("token is not valid")
	}

	// Extract claims
	claims, ok := token.Claims.(jwt.MapClaims)
	if !ok {
		return nil, fmt.Errorf("failed to extract claims from token")
	}

	return claims, nil
}

func getEnv(key, fallback string) string {
	if value, ok := os.LookupEnv(key); ok {
		return value
	}
	return fallback
}
