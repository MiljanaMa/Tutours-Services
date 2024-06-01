package nats

import (
	"github.com/nats-io/nats.go"
	"log"
)

func Conn() *nats.Conn {
	conn, err := nats.Connect("nats://localhost:4222")
	if err != nil {
		log.Fatalln(err)
	}

	return conn
}
