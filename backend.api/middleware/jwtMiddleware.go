package middleware

import (
	"backend.api/utils"

	"github.com/gofiber/fiber/v2"
	"github.com/golang-jwt/jwt"
)

func JWTMiddleware(c *fiber.Ctx) error {
	tokenString := c.Get("Authorization")
	if tokenString == "" || len(tokenString) < 7 || tokenString[:7] != "Bearer " {
		return c.Status(401).JSON(fiber.Map{"error": "missing or invalid token"})
	}

	tokenString = tokenString[7:]

	token, err := utils.ParseJWT(tokenString)
	if err != nil || !token.Valid {
		return c.Status(401).JSON(fiber.Map{"error": "unauthorized"})
	}

	if claims, ok := token.Claims.(jwt.MapClaims); ok && token.Valid {
		c.Locals("userid", claims["id"])
	}

	return c.Next()
}
