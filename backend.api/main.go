package main

import (
	"context"
	"fmt"
	"log"
	"os"
	"strconv"

	"github.com/gofiber/fiber/v2"
	"go.mongodb.org/mongo-driver/bson"
	"go.mongodb.org/mongo-driver/bson/primitive"
	"go.mongodb.org/mongo-driver/mongo"
	"go.mongodb.org/mongo-driver/mongo/options"
	"golang.org/x/crypto/bcrypt"
)

type Book struct {
	Id     primitive.ObjectID `json:"_id,omitempty" bson:"_id,omitempty"`
	UserId primitive.ObjectID `json:"userId"`
	Title  string             `json:"title"`
}

type User struct {
	Id       primitive.ObjectID `json:"_id,omitempty" bson:"_id,omitempty"`
	Name     string             `json:"name"`
	Password string             `json:"password"`
}

var (
	bookCollection *mongo.Collection
	userCollection *mongo.Collection
)

func main() {
	fmt.Println("Starting server...")

	MONGODB_URI, exists := os.LookupEnv("MONGODB_URI")
	if !exists {
		log.Fatal("no mongodb uri found in envionment variable")
	}

	clientOptions := options.Client().ApplyURI(MONGODB_URI)
	client, err := mongo.Connect(context.Background(), clientOptions)
	if err != nil {
		log.Fatal(err)
	}
	defer client.Disconnect(context.Background())

	err = client.Ping(context.Background(), nil)
	if err != nil {
		log.Fatal(err)
	}

	fmt.Println("Connected to MongoDB")

	bookCollection = client.Database("golang_db").Collection("books")
	userCollection = client.Database("golang_db").Collection("users")

	app := fiber.New()

	app.Get("api/books", getBooks)
	app.Post("api/books", createBook)

	app.Post("api/users/register", registerUser)
	app.Post("api/users/login", loginUser)

	PORT := 4000
	log.Fatal(app.Listen(":" + strconv.Itoa(PORT)))
}

func getBooks(c *fiber.Ctx) error {
	var books []Book

	cursor, err := bookCollection.Find(context.Background(), bson.M{})
	if err != nil {
		return err
	}
	defer cursor.Close(context.Background())

	for cursor.Next(context.Background()) {
		var book Book
		if err := cursor.Decode(&book); err != nil {
			return err
		}
		books = append(books, book)
	}

	return c.JSON(books)
}

func createBook(c *fiber.Ctx) error {
	book := new(Book)

	if err := c.BodyParser(book); err != nil {
		return err
	}

	if book.Title == "" {
		return c.Status(400).JSON(fiber.Map{"error": "book title is required"})
	}

	if book.UserId.IsZero() {
		return c.Status(400).JSON(fiber.Map{"error": "userId is required and must be valid"})
	}

	insertResult, err := bookCollection.InsertOne(context.Background(), book)
	if err != nil {
		return err
	}

	book.Id = insertResult.InsertedID.(primitive.ObjectID)
	return c.Status(201).JSON(book)
}

func registerUser(c *fiber.Ctx) error {
	user := new(User)
	if err := c.BodyParser(user); err != nil {
		return err
	}

	if user.Name == "" || user.Password == "" {
		return c.Status(400).JSON(fiber.Map{"error": "name and password are required"})
	}

	hashedPassword, err := bcrypt.GenerateFromPassword([]byte(user.Password), bcrypt.DefaultCost)
	if err != nil {
		return c.Status(500).JSON(fiber.Map{"error": "could not hash password"})
	}
	user.Password = string(hashedPassword)

	insertResult, err := userCollection.InsertOne(context.Background(), user)
	if err != nil {
		return c.Status(500).JSON(fiber.Map{"error": "could not register user"})
	}

	user.Id = insertResult.InsertedID.(primitive.ObjectID)
	return c.Status(201).JSON(fiber.Map{"message": "user registered successfully", "user": user})
}

func loginUser(c *fiber.Ctx) error {
	var input struct {
		Name     string `json:"name"`
		Password string `json:"password"`
	}
	if err := c.BodyParser(&input); err != nil {
		return err
	}

	if input.Name == "" || input.Password == "" {
		return c.Status(400).JSON(fiber.Map{"error": "name and password are required"})
	}

	var user User
	err := userCollection.FindOne(context.Background(), bson.M{"name": input.Name}).Decode(&user)
	if err != nil {
		return c.Status(400).JSON(fiber.Map{"error": "invalid credentials"})
	}

	err = bcrypt.CompareHashAndPassword([]byte(user.Password), []byte(input.Password))
	if err != nil {
		return c.Status(400).JSON(fiber.Map{"error": "invalid credentials"})
	}

	return c.Status(200).JSON(fiber.Map{"message": "login successful"})
}
