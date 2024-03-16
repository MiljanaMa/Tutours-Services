package enum

type TourDifficulty int

const (
	EASY TourDifficulty = iota
	MEDIUM
	HARD
	EXTREME
)

func (td TourDifficulty) ToString() string {
	switch td {
	case EASY:
		return "EASY"
	case MEDIUM:

		return "MEDIUM"
	case HARD:

		return "HARD"
	case EXTREME:

		return "EXTREME"
	default:
		return "UNKNOWN"
	}
}
func FromStringToDifficulty(td string) TourDifficulty {
	switch td {
	case "EASY":
		return EASY
	case "MEDIUM":

		return MEDIUM
	case "HARD":

		return HARD
	case "EXTREME":
		return EXTREME
	default:
		return EASY
	}
}
