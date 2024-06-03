package nats

import (
	"log"
	"os"
	"strconv"

	"github.com/nats-io/nats.go"
)

func Conn() *nats.Conn {
	natsHost := os.Getenv("NATS_HOST")
	natsPortStr := os.Getenv("NATS_PORT")
	natsPort, err := strconv.Atoi(natsPortStr)
	if err != nil {
		log.Fatalf("Error converting NATS_PORT to integer: %v", err)
	}

	conn, err := nats.Connect("nats://" + natsHost + ":" + strconv.Itoa(natsPort))
	if err != nil {
		log.Fatalf("Error connecting to NATS: %v", err)
	}

	return conn
}
