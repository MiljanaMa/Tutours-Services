package model

import (
	"time"
)

type EncounterCompletionStatus string

const (
	CompletionStatusStarted     EncounterCompletionStatus = "STARTED"
	CompletionStatusFailed      EncounterCompletionStatus = "FAILED"
	CompletionStatusCompleted   EncounterCompletionStatus = "COMPLETED"
	CompletionStatusProgressing EncounterCompletionStatus = "PROGRESSING"
)

type EncounterCompletion struct {
	Id            int `gorm:"primaryKey;autoIncrement"`
	UserId        int
	EncounterId   int //`gorm:"foreignKey:EncounterId"`
	LastUpdatedAt time.Time
	Xp            int
	Status        EncounterCompletionStatus
	Encounter     *Encounter
}
