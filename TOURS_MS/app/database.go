package app

import (
	"context"
	"fmt"
	"go.mongodb.org/mongo-driver/mongo"
	"go.mongodb.org/mongo-driver/mongo/options"
	"log"
	"os"
)

func InitDB() *mongo.Client {

	MongoUri := os.Getenv("MONGODB_ADDR")
	MongoUri = "mongodb://localhost:27017/"
	// Connect to the database.
	clientOptions := options.Client().ApplyURI(MongoUri)
	client, err := mongo.Connect(context.Background(), clientOptions)
	if err != nil {
		log.Fatal(err)
	}

	err = client.Ping(context.Background(), nil)
	if err != nil {
		log.Fatal(err)
	} else {
		fmt.Println("Connected to mongoDB!!!")
	}
	return client
}
