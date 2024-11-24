package controllers

import (
	"context"

	"backend.api/models"
	"backend.api/utils"
	"github.com/gofiber/fiber/v2"
	"go.mongodb.org/mongo-driver/bson"
	"go.mongodb.org/mongo-driver/bson/primitive"
	"golang.org/x/crypto/bcrypt"
)

func RegisterUser(c *fiber.Ctx) error {
	user := new(models.User)
	if err := c.BodyParser(user); err != nil {
		return err
	}

	if user.Name == "" || user.Password == "" {
		return c.Status(400).JSON(fiber.Map{"error": "name and password are required"})
	}

	if user.Password != user.ConfirmPassword {
		return c.Status(400).JSON(fiber.Map{"error": "passwords do not match"})
	}

	hashedPassword, err := bcrypt.GenerateFromPassword([]byte(user.Password), bcrypt.DefaultCost)
	if err != nil {
		return c.Status(500).JSON(fiber.Map{"error": "could not hash password"})
	}
	user.Password = string(hashedPassword)

	insertResult, err := utils.UserCollection.InsertOne(context.Background(), user)
	if err != nil {
		return c.Status(500).JSON(fiber.Map{"error": "could not register user", "details": err.Error()})
	}

	user.Id = insertResult.InsertedID.(primitive.ObjectID)
	return c.Status(201).JSON(fiber.Map{"message": "user registered successfully", "user": user})
}

func LoginUser(c *fiber.Ctx) error {
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

	var user models.User
	err := utils.UserCollection.FindOne(context.Background(), bson.M{"name": input.Name}).Decode(&user)
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

	token, err := utils.GenerateJWT(user)
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
