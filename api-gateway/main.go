package main

import (
	"api-gateway/proto/follower"
	"context"
	"github.com/grpc-ecosystem/grpc-gateway/v2/runtime"
	"google.golang.org/grpc"
	"google.golang.org/grpc/credentials/insecure"
	"log"
	"net/http"
	"os"
	"os/signal"
	"syscall"
)

func main() {
	conn, err := grpc.DialContext(
		context.Background(),
		"0.0.0.0:8095",
		grpc.WithBlock(),
		grpc.WithTransportCredentials(insecure.NewCredentials()),
	)
	if err != nil {
		log.Fatalln(err)
	}

	gwmux := runtime.NewServeMux()
	client := follower.NewFollowerServiceClient(conn)
	err = follower.RegisterFollowerServiceHandlerClient(context.Background(), gwmux, client)
	if err != nil {
		log.Fatalln(err)
	}

	gwServer := &http.Server{Addr: ":8099", Handler: gwmux}

	go func() {
		log.Println("Starting HTTP server on port 8099")
		if err := gwServer.ListenAndServe(); err != nil {
			log.Fatalln(err)
		}
	}()

	stopCh := make(chan os.Signal)
	signal.Notify(stopCh, syscall.SIGTERM)
	<-stopCh

	if err = gwServer.Close(); err != nil {
		log.Fatalln(err)
	}
}
