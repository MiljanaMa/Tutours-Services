package main

import (
	"api-gateway/middleware"
	"api-gateway/proto/encounter"
	"api-gateway/proto/stakeholder"
	"api-gateway/proto/tour"
	"api-gateway/utils"
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
	/*conn1, err := grpc.DialContext(
		context.Background(),
		"follower:8095",
		grpc.WithBlock(),
		grpc.WithTransportCredentials(insecure.NewCredentials()),
	)
	if err != nil {
		log.Fatalln(err)
	}
	defer conn1.Close()*/

	conn2, err := grpc.DialContext(
		context.Background(),
		"encounter:8092",
		grpc.WithBlock(),
		grpc.WithTransportCredentials(insecure.NewCredentials()),
	)
	if err != nil {
		log.Fatalln(err)
	}
	defer conn2.Close()

	conn3, err := grpc.DialContext(
		context.Background(),
		"tour:8000",
		grpc.WithBlock(),
		grpc.WithTransportCredentials(insecure.NewCredentials()),
	)
	if err != nil {
		log.Fatalln(err)
	}
	defer conn3.Close()
	conn4, err := grpc.DialContext(
		context.Background(),
		"stakeholder:8099",
		grpc.WithBlock(),
		grpc.WithTransportCredentials(insecure.NewCredentials()),
	)
	if err != nil {
		log.Fatalln(err)
	}
	defer conn4.Close()

	gwmux := runtime.NewServeMux()

	/*client1 := follower.NewFollowerServiceClient(conn1)
	err = follower.RegisterFollowerServiceHandlerClient(context.Background(), gwmux, client1)
	if err != nil {
		log.Fatalln(err)

	}*/
	client2_1 := encounter.NewEncounterServiceClient(conn2)
	client2_2 := encounter.NewEncounterCompletionServiceClient(conn2)
	client2_3 := encounter.NewKeypointEncounterServiceClient(conn2)

	err1 := encounter.RegisterEncounterServiceHandlerClient(context.Background(), gwmux, client2_1)
	err2 := encounter.RegisterEncounterCompletionServiceHandlerClient(context.Background(), gwmux, client2_2)
	err3 := encounter.RegisterKeypointEncounterServiceHandlerClient(context.Background(), gwmux, client2_3)

	if err1 != nil || err2 != nil || err3 != nil {
		log.Fatalln(err)
	}
	client3_1 := tour.NewTourServiceClient(conn3)
	err3_1 := tour.RegisterTourServiceHandlerClient(context.Background(), gwmux, client3_1)
	if err3_1 != nil {
		log.Fatalln(err3_1)

	}
	client4 := stakeholder.NewStakeholderServiceClient(conn4)
	err4 := stakeholder.RegisterStakeholderServiceHandlerClient(context.Background(), gwmux, client4)
	if err4 != nil {
		log.Fatalln(err4)

	}
	handler := middleware.JwtMiddleware(gwmux, utils.GetProtectedPaths())
	gwServer := &http.Server{Addr: ":44333", Handler: handler}
	gwServer.Handler = enableCors(gwmux)
	go func() {
		log.Println("Starting HTTP server on port 44333")
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
func setupGateway() http.Handler {

	gwmux := runtime.NewServeMux()

	conn2, err := grpc.DialContext(
		context.Background(),
		"encounter:8092",
		grpc.WithBlock(),
		grpc.WithTransportCredentials(insecure.NewCredentials()),
	)
	if err != nil {
		log.Fatalln(err)
	}
	defer conn2.Close()

	conn3, err := grpc.DialContext(
		context.Background(),
		"tour:8000",
		grpc.WithBlock(),
		grpc.WithTransportCredentials(insecure.NewCredentials()),
	)
	if err != nil {
		log.Fatalln(err)
	}
	defer conn3.Close()

	conn4, err := grpc.DialContext(
		context.Background(),
		"stakeholder:8099",
		grpc.WithBlock(),
		grpc.WithTransportCredentials(insecure.NewCredentials()),
	)
	if err != nil {
		log.Fatalln(err)
	}
	defer conn4.Close()

	client2_1 := encounter.NewEncounterServiceClient(conn2)
	client2_2 := encounter.NewEncounterCompletionServiceClient(conn2)
	client2_3 := encounter.NewKeypointEncounterServiceClient(conn2)

	err1 := encounter.RegisterEncounterServiceHandlerClient(context.Background(), gwmux, client2_1)
	err2 := encounter.RegisterEncounterCompletionServiceHandlerClient(context.Background(), gwmux, client2_2)
	err3 := encounter.RegisterKeypointEncounterServiceHandlerClient(context.Background(), gwmux, client2_3)

	if err1 != nil || err2 != nil || err3 != nil {
		log.Fatalln(err)
	}
	client3_1 := tour.NewTourServiceClient(conn3)
	err3_1 := tour.RegisterTourServiceHandlerClient(context.Background(), gwmux, client3_1)
	if err3_1 != nil {
		log.Fatalln(err3_1)

	}
	client4 := stakeholder.NewStakeholderServiceClient(conn4)
	err4 := stakeholder.RegisterStakeholderServiceHandlerClient(context.Background(), gwmux, client4)
	if err4 != nil {
		log.Fatalln(err4)

	}

	return middleware.JwtMiddleware(addCorsMiddleware(gwmux), utils.GetProtectedPaths())
}
func addCorsMiddleware(handler http.Handler) http.Handler {
	return http.HandlerFunc(func(w http.ResponseWriter, r *http.Request) {

		w.Header().Set("Access-Control-Allow-Origin", "*")
		w.Header().Set("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS")
		w.Header().Set("Access-Control-Allow-Headers", "Content-Type, Authorization")

		if r.Method == "OPTIONS" {
			w.WriteHeader(http.StatusOK)
			return
		}

		handler.ServeHTTP(w, r)
	})
}
func enableCors(h http.Handler) http.Handler {
	return http.HandlerFunc(func(w http.ResponseWriter, r *http.Request) {
		w.Header().Set("Access-Control-Allow-Origin", "*")

		h.ServeHTTP(w, r)
	})
}
