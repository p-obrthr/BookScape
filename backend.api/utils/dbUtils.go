package utils

import (
	"context"
	"log"
	"os"

	"go.mongodb.org/mongo-driver/mongo"
	"go.mongodb.org/mongo-driver/mongo/options"
)

var (
	BookCollection *mongo.Collection
	UserCollection *mongo.Collection
)

func InitDb() {
	MONGODB_URI, exists := os.LookupEnv("MONGODB_URI")
	if !exists {
		log.Fatal("no mongodb uri found in environment variable")
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

	BookCollection = client.Database("golang_db").Collection("books")
	UserCollection = client.Database("golang_db").Collection("users")
}
