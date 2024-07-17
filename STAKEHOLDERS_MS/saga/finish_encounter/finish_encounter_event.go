package finish_encounter

type EncounterDetails struct {
	Id     int
	Xp     int
	UserId int
}
type FinishEncounterCommandType int8

const (
	AddXp FinishEncounterCommandType = iota
	RollbackXP
	FinishEncounter
	NotFinishEncounter
	UnknownCommand
)

type FinishEncounterCommand struct {
	Encounter EncounterDetails
	Type      FinishEncounterCommandType
}

type FinishEncounterReplyType int8

const (
	XpAdded FinishEncounterReplyType = iota
	XpNotAdded
	XpRolledBack
	EncounterFinished
	EncounterNotFinished
	UnknownReply
)

type FinishEncounterReply struct {
	Encounter EncounterDetails
	Type      FinishEncounterReplyType
}
