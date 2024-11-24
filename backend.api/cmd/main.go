package main

import (
	"log"
	"strconv"

	"backend.api/controllers"
	"backend.api/middleware"
	"backend.api/utils"
	"github.com/gofiber/fiber/v2"
)

func main() {
	utils.InitDb()

	utils.InitJWT()

	app := fiber.New()

	app.Get("/api/books", middleware.JWTMiddleware, controllers.GetBooks)
	app.Post("/api/books", middleware.JWTMiddleware, controllers.CreateBook)

	app.Post("/api/users/register", controllers.RegisterUser)
	app.Post("/api/users/login", controllers.LoginUser)

	log.Printf("Server started on port %d...\n", 4000)
	log.Fatal(app.Listen(":" + strconv.Itoa(4000)))
}
