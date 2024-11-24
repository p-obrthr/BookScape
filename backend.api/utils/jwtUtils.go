package utils

import (
	"fmt"
	"log"
	"os"
	"time"

	"backend.api/models"

	"github.com/golang-jwt/jwt"
)

var jwtKeyBytes []byte

func InitJWT() {
	JWT_KEY, exists := os.LookupEnv("JWT_KEY")
	if !exists {
		log.Fatal("no jwt key found in environment variable")
	}
	jwtKeyBytes = []byte(JWT_KEY)
}

func GenerateJWT(user models.User) (string, error) {
	claims := jwt.MapClaims{
		"id":  user.Id.Hex(),
		"exp": time.Now().Add(time.Hour * 24).Unix(),
	}

	token := jwt.NewWithClaims(jwt.SigningMethodHS256, claims)
	return token.SignedString(jwtKeyBytes)
}

func ParseJWT(tokenString string) (*jwt.Token, error) {
	token, err := jwt.Parse(tokenString, func(token *jwt.Token) (interface{}, error) {
		if _, ok := token.Method.(*jwt.SigningMethodHMAC); !ok {
			return nil, fmt.Errorf("unexpected signing method: %v", token.Header["alg"])
		}
		return jwtKeyBytes, nil
	})
	return token, err
}
