package controllers

import (
	"context"

	"backend.api/models"
	"backend.api/utils"

	"github.com/gofiber/fiber/v2"
	"go.mongodb.org/mongo-driver/bson"
	"go.mongodb.org/mongo-driver/bson/primitive"
)

func GetBooks(c *fiber.Ctx) error {
	userId := c.Locals("userid").(string)
	objectId, err := primitive.ObjectIDFromHex(userId)
	if err != nil {
		return c.Status(500).JSON(fiber.Map{"error": "invalid userId"})
	}

	filter := bson.M{"userid": objectId}
	cursor, err := utils.BookCollection.Find(context.Background(), filter)
	if err != nil {
		return err
	}
	defer cursor.Close(context.Background())

	var books []models.Book
	for cursor.Next(context.Background()) {
		var book models.Book
		if err := cursor.Decode(&book); err != nil {
			return err
		}
		books = append(books, book)
	}

	return c.JSON(books)
}

func CreateBook(c *fiber.Ctx) error {
	book := new(models.Book)

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

	insertResult, err := utils.BookCollection.InsertOne(context.Background(), book)
	if err != nil {
		return err
	}

	book.Id = insertResult.InsertedID.(primitive.ObjectID)
	return c.Status(201).JSON(book)
}
