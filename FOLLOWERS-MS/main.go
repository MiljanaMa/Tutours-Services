package main

import (
	"FOLLOWERS-MS/handler"
	"FOLLOWERS-MS/repo"
	"FOLLOWERS-MS/service"
	"context"
	"fmt"
	"log"
	"net/http"
	"time"

	"github.com/gorilla/mux"
	"github.com/neo4j/neo4j-go-driver/v5/neo4j"
)

func startServer(handler *handler.FollowersHandler) {

	router := mux.NewRouter().StrictSlash(true)
	url := "/followers/"
	router.HandleFunc(url+"follow/{id1}/{id2}", handler.Follow).Methods("POST")
	router.HandleFunc(url+"check-if-following/{id1}/{id2}", handler.CheckIfFollowing).Methods("GET")
	router.HandleFunc(url+"get-recommendations/{id}", handler.GetRecommendation).Methods("GET")
	router.HandleFunc(url+"unfollow/{id1}/{id2}", handler.Unfollow).Methods("DELETE")
	router.HandleFunc(url+"get-followings/{id}", handler.GetFollowings).Methods("GET")
	router.HandleFunc(url+"get-followers/{id}", handler.GetFollowers).Methods("GET")

	fmt.Println("Starting server")
	log.Fatal(http.ListenAndServe(":8095", router))
}
func initDB() neo4j.DriverWithContext {

	uri := "bolt://localhost:7687"
	user := "neo4j"
	pass := "12345678"
	auth := neo4j.BasicAuth(user, pass, "")

	driver, err := neo4j.NewDriverWithContext(uri, auth)
	if err != nil {
		fmt.Println(err)
		return nil

	}

	return driver

}
func CheckConnection(driver neo4j.DriverWithContext) {
	ctx := context.Background()
	err := driver.VerifyConnectivity(ctx)
	if err != nil {
		fmt.Println(err)
		return
	}
	fmt.Printf("Neo4J server address: %s \n", driver.Target().Host)
}
func main() {
	timeoutContext, cancel := context.WithTimeout(context.Background(), 90*time.Second)
	defer cancel()

	driver := initDB()

	defer driver.Close(timeoutContext)
	CheckConnection(driver)

	if driver == nil {
		return
	}
	followerRepository := &repo.FollowerRepository{driver}
	followerService := &service.FollowerService{followerRepository}
	followerHandler := &handler.FollowersHandler{followerService}

	startServer(followerHandler)
}
