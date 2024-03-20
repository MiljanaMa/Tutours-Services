package model

import (
	"time"

	"github.com/google/uuid"
)

type EncounterCompletionStatus string

const (
	CompletionStatusStarted     EncounterCompletionStatus = "STARTED"
	CompletionStatusFailed      EncounterCompletionStatus = "FAILED"
	CompletionStatusCompleted   EncounterCompletionStatus = "COMPLETED"
	CompletionStatusProgressing EncounterCompletionStatus = "PROGRESSING"
)

type EncounterCompletion struct {
	Id            uuid.UUID `gorm:"type:uuid;primaryKey"`
	UserId        int
	EncounterId   uuid.UUID `gorm:"type:uuid"`
	LastUpdatedAt time.Time
	Xp            int
	Status        EncounterCompletionStatus
	Encounter     *Encounter
}
