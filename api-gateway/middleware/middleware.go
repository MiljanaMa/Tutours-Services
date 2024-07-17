package middleware

import (
	"api-gateway/utils"
	"context"
	"fmt"
	"net/http"
	"strings"
	"time"

	"github.com/dgrijalva/jwt-go"
)

var jwtKey = []byte("explorer_secret_key")

func JwtMiddleware(next http.Handler, protectedPaths []*utils.Path) http.Handler {
	fmt.Println("da")
	return http.HandlerFunc(func(w http.ResponseWriter, r *http.Request) {
		fmt.Println("ne")
		protected, role := isProtectedPath(r.URL.Path, protectedPaths)
		fmt.Println(protected, role)
		tokenString := r.Header.Get("Authorization")
		tokenString = strings.TrimPrefix(tokenString, "Bearer ")
		fmt.Println(tokenString)
		if !protected {

			ctx := context.WithValue(r.Context(), "jwtToken", tokenString)
			next.ServeHTTP(w, r.WithContext(ctx))
			return
		}

		if tokenString == "" {
			http.Error(w, "Authorization token is missing", http.StatusUnauthorized)
			return
		}

		// Parse and validate JWT token
		token, err := jwt.ParseWithClaims(tokenString, &utils.Claims{}, func(token *jwt.Token) (interface{}, error) {
			if _, ok := token.Method.(*jwt.SigningMethodHMAC); !ok {
				return nil, fmt.Errorf("unexpected signing method: %v", token.Header["alg"])
			}
			// Return the key for validation
			return []byte(getEnv("JWT_KEY", "explorer_secret_key")), nil
		})
		fmt.Println(token)
		if err != nil || !token.Valid {
			http.Error(w, "Invalid token", http.StatusUnauthorized)
			return
		}

		claims, ok := token.Claims.(*utils.Claims)
		if !ok {
			http.Error(w, "Invalid claims format", http.StatusUnauthorized)
			return
		}

		if role != "" && role != claims.Role {
			http.Error(w, "Role cannot access endpoint", http.StatusUnauthorized)
			return
		}

		if time.Now().Unix() > claims.ExpiresAt {
			http.Error(w, "Token has expired", http.StatusUnauthorized)
			return
		}

		fmt.Println(token)
		// Attach parsed token to request context
		ctx := context.WithValue(r.Context(), "jwtToken", token)

		next.ServeHTTP(w, r.WithContext(ctx))
	})
}
func getEnv(key, fallback string) string {
	// Dummy function for illustration
	return fallback
}

func isProtectedPath(path string, protectedPaths []*utils.Path) (bool, string) {
	for _, p := range protectedPaths {
		if strings.Contains(path, p.Path) {
			return true, p.Role
		}
	}
	return false, ""
}
