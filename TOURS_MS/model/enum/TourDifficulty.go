package enum

type TourDifficulty int

const (
	EASY TourDifficulty = iota
	MEDIUM
	HARD
	EXTREME
)

/*
func (d TourDifficulty) ToString() string {
	switch d {
	case EASY:
		return "Easy"
	case MEDIUM:
		return "Medium"
	case HARD:
		return "Hard"
	case EXTREME:
		return "Extreme"
	default:
		return "Unknown"
	}
}*/
