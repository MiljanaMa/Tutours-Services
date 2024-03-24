package enum

type TourStatus int

const (
	DRAFT TourStatus = iota
	PUBLISHED
	ARCHIVED
	DISABLED
	CUSTOM
	CAMPAIGN
)

func (ts TourStatus) ToString() string {
	switch ts {
	case DRAFT:
		return "DRAFT"
	case PUBLISHED:

		return "PUBLISHED"
	case ARCHIVED:

		return "ARCHIVED"
	case DISABLED:

		return "DISABLED"
	case CUSTOM:

		return "CUSTOM"
	case CAMPAIGN:

		return "CAMPAIGN"
	default:
		return "UNKNOWN"
	}
}
func FromStringToStatus(ts string) TourStatus {
	switch ts {
	case "DRAFT":
		return DRAFT
	case "PUBLISHED":

		return PUBLISHED
	case "ARCHIVED":

		return ARCHIVED
	case "DISABLED":
		return DISABLED
	case "CUSTOM":

		return CUSTOM
	case "CAMPAIGN":

		return CAMPAIGN
	default:
		return DRAFT
	}
}
