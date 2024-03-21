package model

import "github.com/google/uuid"

type KeypointEncounter struct {
	Id          uuid.UUID `gorm:"type:uuid;primaryKey"`
	EncounterId uuid.UUID `gorm:"type:uuid"`
	Encounter   *Encounter
	KeyPointId  int
	IsRequired  bool
}
