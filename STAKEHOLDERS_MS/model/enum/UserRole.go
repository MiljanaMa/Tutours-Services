package enum

type UserRole int

const (
	Administrator UserRole = iota
	Author
	Tourist
)
