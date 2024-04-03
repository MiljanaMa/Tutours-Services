package model

type KeypointEncounter struct {
	Id          int `gorm:"primaryKey;autoIncrement"`
	EncounterId int //`gorm:"foreignKey:EncounterId"`
	Encounter   *Encounter
	KeyPointId  int
	IsRequired  bool
}
