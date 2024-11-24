package models

import "go.mongodb.org/mongo-driver/bson/primitive"

type Book struct {
	Id     primitive.ObjectID `json:"_id,omitempty" bson:"_id,omitempty"`
	UserId primitive.ObjectID `json:"userid"`
	Title  string             `json:"title"`
}
