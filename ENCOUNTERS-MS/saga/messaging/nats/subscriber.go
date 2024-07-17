package nats

import (
	saga "ENCOUNTERS-MS/saga/messaging"
	"github.com/nats-io/nats.go"
)

type Subscriber struct {
	conn       *nats.EncodedConn
	subject    string
	queueGroup string
}

func NewSubscriber(subject, queueGroup string) (saga.Subscriber, error) {
	conn := Conn()
	encConn, err := nats.NewEncodedConn(conn, nats.JSON_ENCODER)
	if err != nil {
		return nil, err
	}
	return &Subscriber{encConn, subject, queueGroup}, nil
}

func (s *Subscriber) Subscribe(handler interface{}) error {
	_, err := s.conn.QueueSubscribe(s.subject, s.queueGroup, handler)
	if err != nil {
		return err
	}
	return nil
}
