package model

import "github.com/google/uuid"

type EncounterStatus string

const (
	StatusActive   EncounterStatus = "ACTIVE"
	StatusDraft    EncounterStatus = "DRAFT"
	StatusArchived EncounterStatus = "ARCHIVED"
)

type EncounterApprovalStatus string

const (
	ApprovalPending        EncounterApprovalStatus = "PENDING"
	ApprovalSystemApproved EncounterApprovalStatus = "SYSTEM_APPROVED"
	ApprovalAdminApproved  EncounterApprovalStatus = "ADMIN_APPROVED"
	ApprovalDeclined       EncounterApprovalStatus = "DECLINED"
)

type EncounterType string

const (
	TypeSocial   EncounterType = "SOCIAL"
	TypeLocation EncounterType = "LOCATION"
	TypeMisc     EncounterType = "MISC"
)

type Encounter struct {
	ID             uuid.UUID
	UserID         int
	Name           string
	Description    string
	Latitude       float64
	Longitude      float64
	Xp             int
	Status         EncounterStatus
	Type           EncounterType
	Range          float64
	Image          string
	ImageLatitude  *float64
	ImageLongitude *float64
	PeopleCount    int
	ApprovalStatus EncounterApprovalStatus
}
