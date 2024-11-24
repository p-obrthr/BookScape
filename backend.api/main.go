package main

import (
	"context"
	"fmt"
	"log"
	"os"
	"strconv"
	"time"

	"github.com/gofiber/fiber/v2"
	"github.com/golang-jwt/jwt"
	"go.mongodb.org/mongo-driver/bson"
	"go.mongodb.org/mongo-driver/bson/primitive"
	"go.mongodb.org/mongo-driver/mongo"
	"go.mongodb.org/mongo-driver/mongo/options"
	"golang.org/x/crypto/bcrypt"
)

type Book struct {
	Id     primitive.ObjectID `json:"_id,omitempty" bson:"_id,omitempty"`
	UserId primitive.ObjectID `json:"userid"`
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

var jwtKeyBytes []byte

func main() {

	JWT_KEY, exists := os.LookupEnv("JWT_KEY")
	if !exists {
		log.Fatal("no jwt key found in environment variable")
	}

	jwtKeyBytes = []byte(JWT_KEY)

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

	app.Get("api/books", jwtMiddleware, getBooks)
	app.Post("api/books", jwtMiddleware, createBook)

	app.Post("api/users/register", registerUser)
	app.Post("api/users/login", loginUser)

	fmt.Println("Starting server...")

	PORT := 4000
	log.Fatal(app.Listen(":" + strconv.Itoa(PORT)))
}

func generateJWT(user User) (string, error) {
	claims := jwt.MapClaims{
		"id":  user.Id.Hex(),
		"exp": time.Now().Add(time.Hour * 24).Unix(),
	}

	token := jwt.NewWithClaims(jwt.SigningMethodHS256, claims)
	return token.SignedString(jwtKeyBytes)
}

func jwtMiddleware(c *fiber.Ctx) error {
	tokenString := c.Get("Authorization")
	if tokenString == "" || len(tokenString) < 7 || tokenString[:7] != "Bearer " {
		return c.Status(401).JSON(fiber.Map{"error": "missing or invalid token"})
	}

	tokenString = tokenString[7:]

	token, err := jwt.Parse(tokenString, func(token *jwt.Token) (interface{}, error) {
		if _, ok := token.Method.(*jwt.SigningMethodHMAC); !ok {
			return nil, fmt.Errorf("unexpected signing method: %v", token.Header["alg"])
		}
		return jwtKeyBytes, nil
	})

	if err != nil || !token.Valid {
		return c.Status(401).JSON(fiber.Map{"error": "unauthorized"})
	}

	if claims, ok := token.Claims.(jwt.MapClaims); ok && token.Valid {
		c.Locals("userid", claims["id"])
	}

	return c.Next()
}

func getBooks(c *fiber.Ctx) error {
	userId := c.Locals("userid").(string)
	objectId, err := primitive.ObjectIDFromHex(userId)
	if err != nil {
		return c.Status(500).JSON(fiber.Map{"error": "invalid userId"})
	}

	filter := bson.M{"userid": objectId}
	cursor, err := bookCollection.Find(context.Background(), filter)
	if err != nil {
		return err
	}
	defer cursor.Close(context.Background())

	var books []Book
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

	userId := c.Locals("userid").(string)
	objectId, err := primitive.ObjectIDFromHex(userId)
	if err != nil {
		return c.Status(500).JSON(fiber.Map{"error": "invalid userId"})
	}
	book.UserId = objectId

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
		return c.Status(400).JSON(fiber.Map{
			"flag":    false,
			"message": "name and password are required",
		})
	}

	var user User
	err := userCollection.FindOne(context.Background(), bson.M{"name": input.Name}).Decode(&user)
	if err != nil {
		return c.Status(400).JSON(fiber.Map{
			"flag":    false,
			"message": "invalid credentials",
		})
	}

	err = bcrypt.CompareHashAndPassword([]byte(user.Password), []byte(input.Password))
	if err != nil {
		return c.Status(400).JSON(fiber.Map{
			"flag":    false,
			"message": "invalid credentials",
		})
	}

	token, err := generateJWT(user)
	if err != nil {
		return c.Status(500).JSON(fiber.Map{
			"flag":    false,
			"message": "could not generate token",
		})
	}

	return c.Status(200).JSON(fiber.Map{
		"flag":    true,
		"message": "login successful",
		"token":   token,
	})
}
