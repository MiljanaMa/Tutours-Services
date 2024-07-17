package main

import (
	"FOLLOWERS-MS/handler"
	"FOLLOWERS-MS/proto/follower"
	"FOLLOWERS-MS/repo"
	"FOLLOWERS-MS/service"
	"context"
	"fmt"
	"github.com/neo4j/neo4j-go-driver/v5/neo4j"
	"google.golang.org/grpc"
	"google.golang.org/grpc/reflection"
	"log"
	"net"
	"os"
	"os/signal"
	"syscall"
	"time"
)

func initDB() neo4j.DriverWithContext {

	/*uri := os.Getenv("NEO4J_DB")
	user := os.Getenv("NEO4J_USERNAME")
	pass := os.Getenv("NEO4J_PASS")*/
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

func cleanData(driver neo4j.DriverWithContext) error {

	ctx := context.Background()
	session := driver.NewSession(ctx, neo4j.SessionConfig{DatabaseName: "neo4j"})
	defer session.Close(ctx)

	_, err := session.ExecuteWrite(ctx,
		func(transaction neo4j.ManagedTransaction) (any, error) {
			_, err := transaction.Run(
				ctx,
				`MATCH (n) DETACH DELETE n`,
				nil)
			return nil, err
		})
	return err
}

func migrateData(driver neo4j.DriverWithContext) error {

	ctx := context.Background()
	session := driver.NewSession(ctx, neo4j.SessionConfig{DatabaseName: "neo4j"})
	defer session.Close(ctx)

	_, err := session.ExecuteWrite(ctx,
		func(transaction neo4j.ManagedTransaction) (any, error) {
			_, err := transaction.Run(
				ctx,
				`MERGE (u1: User {id: 1})
				 MERGE (u2: User {id: 2})
				 MERGE (u3: User {id: 3})
				 MERGE (u4: User {id: 4})
				 MERGE (u5: User {id: 5})
				 MERGE (u6: User {id: 6})
				 MERGE (u7: User {id: 7})
				 MERGE (u8: User {id: 8})
				 CREATE (u1) -[:Following]->(u2)
				 CREATE (u1) -[:Following]->(u3)
				 CREATE (u1) -[:Following]->(u6)
				 CREATE (u3) -[:Following]->(u2)
				 CREATE (u3) -[:Following]->(u4)
				 CREATE (u3) -[:Following]->(u5)
				 CREATE (u4) -[:Following]->(u1)
				 CREATE (u6) -[:Following]->(u2)
				 CREATE (u6) -[:Following]->(u7)
				 CREATE (u6) -[:Following]->(u8)
				 CREATE (u8) -[:Following]->(u1)
				 `,
				nil)
			return nil, err
		})
	return err
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

	cleanData(driver)
	migrateData(driver)

	followerRepository := &repo.FollowerRepository{driver}
	followerService := &service.FollowerService{followerRepository}
	followerHandler := &handler.FollowersHandler{FollowerService: followerService}

	//startServer(followerHandler)
	lis, err := net.Listen("tcp", ":8095")
	if err != nil {
		log.Fatalln(err)
	}

	defer func(listener net.Listener) {
		err := listener.Close()
		if err != nil {
			log.Fatalln(err)
		}
	}(lis)

	grpcServer := grpc.NewServer()
	reflection.Register(grpcServer)

	follower.RegisterFollowerServiceServer(grpcServer, followerHandler)

	go func() {
		if err := grpcServer.Serve(lis); err != nil {
			log.Fatalln(err)
		}
	}()

	stopCh := make(chan os.Signal)
	signal.Notify(stopCh, syscall.SIGTERM)

	<-stopCh

	grpcServer.Stop()

}
