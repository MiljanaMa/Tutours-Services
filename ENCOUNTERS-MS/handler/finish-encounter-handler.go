package handler

import (
	"ENCOUNTERS-MS/model"
	"ENCOUNTERS-MS/saga/finish_encounter"
	saga "ENCOUNTERS-MS/saga/messaging"
	"ENCOUNTERS-MS/service"
)

type FinishEncounterCommandHandler struct {
	service           *service.EncounterCompletionService
	replyPublisher    saga.Publisher
	commandSubscriber saga.Subscriber
}

func NewFinishEncounterCommandHandler(service *service.EncounterCompletionService, publisher saga.Publisher, subscriber saga.Subscriber) (*FinishEncounterCommandHandler, error) {
	h := &FinishEncounterCommandHandler{
		service:           service,
		replyPublisher:    publisher,
		commandSubscriber: subscriber,
	}
	err := h.commandSubscriber.Subscribe(h.handle)
	if err != nil {
		return nil, err
	}
	return h, nil
}
func (handler *FinishEncounterCommandHandler) handle(command *finish_encounter.FinishEncounterCommand) {
	completion := model.EncounterCompletion{Id: command.Encounter.Id}
	reply := finish_encounter.FinishEncounterReply{Encounter: command.Encounter}

	switch command.Type {
	case finish_encounter.FinishEncounter:
		err := handler.service.Finish(&completion)
		if err != nil {
			return
		}
		reply.Type = finish_encounter.EncounterFinished
	case finish_encounter.NotFinishEncounter:
		err := handler.service.NotFinish(&completion)
		if err != nil {
			return
		}
		reply.Type = finish_encounter.EncounterNotFinished
	default:
		reply.Type = finish_encounter.UnknownReply
	}
	if reply.Type != finish_encounter.UnknownReply {
		_ = handler.replyPublisher.Publish(reply)
	}

}
