package handler

import (
	"stakeholder/saga/finish_encounter"
	saga "stakeholder/saga/messaging"
	"stakeholder/service"
)

type FinishEncounterCommandHandler struct {
	service           *service.UserService
	replyPublisher    saga.Publisher
	commandSubscriber saga.Subscriber
}

func NewFinishEncounterCommandHandler(userService *service.UserService, publisher saga.Publisher, subscriber saga.Subscriber) (*FinishEncounterCommandHandler, error) {
	h := &FinishEncounterCommandHandler{
		service:           userService,
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
	reply := finish_encounter.FinishEncounterReply{Encounter: command.Encounter}
	switch command.Type {
	case finish_encounter.AddXp:
		err := handler.service.AddXp(command.Encounter.UserId, command.Encounter.Xp)
		if err != nil {
			reply.Type = finish_encounter.XpNotAdded
			break
		}
		reply.Type = finish_encounter.XpAdded
	case finish_encounter.RollbackXP:
		err := handler.service.AddXp(command.Encounter.UserId, -command.Encounter.Xp)
		if err != nil {
			return
		}
		reply.Type = finish_encounter.XpRolledBack
	default:
		reply.Type = finish_encounter.UnknownReply
	}
	if reply.Type != finish_encounter.UnknownReply {
		_ = handler.replyPublisher.Publish(reply)
	}
}
