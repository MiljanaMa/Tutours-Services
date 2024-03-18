package enum

type TransportType int

const (
	WALK TransportType = iota
	BIKE
	CAR
	BOAT
)

func (tt TransportType) ToString() string {
	switch tt {
	case WALK:
		return "WALK"
	case BIKE:

		return "BIKE"
	case CAR:

		return "CAR"
	case BOAT:

		return "BOAT"
	default:
		return "UNKNOWN"
	}
}
func FromStringToType(tt string) TransportType {
	switch tt {
	case "WALK":
		return WALK
	case "BIKE":

		return BIKE
	case "CAR":

		return CAR
	case "BOAT":
		return BOAT
	default:
		return WALK
	}
}
