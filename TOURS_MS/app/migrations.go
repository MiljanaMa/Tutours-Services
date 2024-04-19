package app

import (
	"context"
	"fmt"
	"go.mongodb.org/mongo-driver/mongo"
	"log"
	"time"
)

func InsertInfo(client *mongo.Client) {
	// Create a context with a timeout of 5 seconds.
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Second)
	defer cancel()

	// Access the "tourService" database and "tours" collection.
	toursDatabase := client.Database("tourService")
	toursCollection := toursDatabase.Collection("tours")
	counter := toursDatabase.Collection("counter")

	// Define the documents to insert.
	tours := []interface{}{
		map[string]interface{}{
			"_id":                1,
			"user_id":            16,
			"name":               "Zlatibor Nature Escape",
			"description":        "Discover the natural beauty of Zlatibor.",
			"price":              1500,
			"duration":           37,
			"distance":           7,
			"difficulty":         2,
			"transport_type":     3,
			"status":             0,
			"status_update_time": time.Date(2024, time.February, 16, 0, 0, 0, 0, time.UTC),
			"tags":               []string{"nature", "escape", "Zlatibor"},
		},
		map[string]interface{}{
			"_id":                2,
			"user_id":            18,
			"name":               "Zlatibor Nature Escape2",
			"description":        "Natural beauty of Zlatibor.",
			"price":              1400,
			"duration":           33,
			"distance":           7,
			"difficulty":         2,
			"transport_type":     3,
			"status":             0,
			"status_update_time": time.Date(2024, time.February, 16, 0, 0, 0, 0, time.UTC),
			"tags":               []string{"nature", "escape", "Zlatibor"},
		},
	}

	// Insert documents into MongoDB collection.
	_, err := toursCollection.InsertMany(ctx, tours)
	if err != nil {
		log.Fatal(err)
	}

	fmt.Println("Documents inserted successfully.")
	counterDoc := map[string]interface{}{
		"_id":   1, // Assuming "_id" is "tourCounter" for the counter document.
		"value": 3, // Initial value for the counter.
	}

	// Insert document into the "counter" collection.
	_, err = counter.InsertOne(ctx, counterDoc)
	if err != nil {
		log.Fatal(err)
	}

	fmt.Println("Document inserted into 'counter' collection successfully.")
}
