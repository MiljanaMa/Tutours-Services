// Connect to MongoDB
const MongoClient = require('mongodb').MongoClient;
const url = "mongodb://localhost:27017"; // MongoDB connection URL

// Function to perform database initialization
async function initializeDatabase() {
    try {
        // Connect to the MongoDB server
        const client = await MongoClient.connect(url, { useNewUrlParser: true, useUnifiedTopology: true });
        console.log("Connected to MongoDB server");

        // Access the database
        const db = client.db();

        // Create collection
        console.log("Creating collection...");
        await db.createCollection('tours', { capped: false });
        console.log("Collection 'tours' created successfully");

        // Insert sample documents
        console.log("Inserting sample documents...");
        await db.collection('tours').insertMany([
            {
                "_id": 1,
                "user_id": 16,
                "name": "Zlatibor Nature Escape",
                "description": "Discover the natural beauty of Zlatibor.",
                "price": 1500,
                "duration": 37,
                "distance": 7,
                "difficulty": 2,
                "transport_type": 3,
                "status": 0,
                "status_update_time": new Date("2024-02-16T00:00:00Z"),
                "tags": ["nature", "escape", "Zlatibor"]
            },
            {
                "_id": 2,
                "user_id": 18,
                "name": "Zlatibor Nature Escape2",
                "description": "Natural beauty of Zlatibor.",
                "price": 1400,
                "duration": 33,
                "distance": 7,
                "difficulty": 2,
                "transport_type": 3,
                "status": 0,
                "status_update_time": new Date("2024-02-16T00:00:00Z"),
                "tags": ["nature", "escape", "Zlatibor"]
            }
        ]);
        console.log("Sample documents inserted successfully");

        // Close the connection
        client.close();
        console.log("Connection to MongoDB closed");
    } catch (error) {
        console.error("An error occurred:", error);
    }
}

// Call the initialization function
initializeDatabase();
