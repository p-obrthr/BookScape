package main

import (
	"context"
	"log"
	"os"
	"os/signal"
	"strconv"
	"syscall"

	"backend.api/controllers"
	"backend.api/middleware"
	"backend.api/utils"
	"github.com/gofiber/fiber/v2"
	"go.mongodb.org/mongo-driver/mongo"
)

var client *mongo.Client

func main() {
	client = utils.InitDb()

	utils.InitJWT()

	app := fiber.New()

	app.Get("/api/books", middleware.JWTMiddleware, controllers.GetBooks)
	app.Post("/api/books", middleware.JWTMiddleware, controllers.CreateBook)
	app.Post("/api/users/register", controllers.RegisterUser)
	app.Post("/api/users/login", controllers.LoginUser)

	shutdown := make(chan os.Signal, 1)
	signal.Notify(shutdown, syscall.SIGINT, syscall.SIGTERM)

	go func() {
		log.Printf("Server started on port %d...\n", 4000)
		if err := app.Listen(":" + strconv.Itoa(4000)); err != nil {
			log.Fatalf("Error starting server: %v", err)
		}
	}()

	<-shutdown

	client.Disconnect(context.Background())
}
