package service

import (
	"ENCOUNTERS-MS/model"
	"ENCOUNTERS-MS/saga/finish_encounter"
	saga "ENCOUNTERS-MS/saga/messaging"
)

type FinishEncounterOrchestrator struct {
	commandPublisher saga.Publisher
	replySubscriber  saga.Subscriber
}

func NewFinishOrderOrchestrator(publisher saga.Publisher, subscriber saga.Subscriber) (*FinishEncounterOrchestrator, error) {
	o := &FinishEncounterOrchestrator{publisher, subscriber}
	err := o.replySubscriber.Subscribe(o.handle)
	if err != nil {
		return nil, err
	}
	return o, nil
}
func (o *FinishEncounterOrchestrator) Start(encounter *model.EncounterCompletion) error {
	event := &finish_encounter.FinishEncounterCommand{
		Encounter: finish_encounter.EncounterDetails{Id: encounter.Id, Xp: encounter.Xp, UserId: encounter.UserId},
		Type:      finish_encounter.AddXp,
	}
	return o.commandPublisher.Publish(event)
}
func (o *FinishEncounterOrchestrator) handle(reply *finish_encounter.FinishEncounterReply) {
	command := finish_encounter.FinishEncounterCommand{Encounter: reply.Encounter}
	command.Type = o.nextCommandType(reply.Type)
	if command.Type != finish_encounter.UnknownCommand {
		_ = o.commandPublisher.Publish(command)
	}
}

func (o *FinishEncounterOrchestrator) nextCommandType(reply finish_encounter.FinishEncounterReplyType) finish_encounter.FinishEncounterCommandType {
	switch reply {
	case finish_encounter.XpAdded:
		return finish_encounter.FinishEncounter
	case finish_encounter.XpNotAdded:
		return finish_encounter.NotFinishEncounter
	case finish_encounter.XpRolledBack:
		return finish_encounter.NotFinishEncounter
	default:
		return finish_encounter.UnknownCommand
	}
}
