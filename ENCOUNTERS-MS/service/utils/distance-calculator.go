package utils

import "math"

var earthRadius int = 6371

func CalculateDistance(lat1 float64, lon1 float64, lat2 float64, lon2 float64) float64 {
	lat1Rad := degreeToRadians(lat1)
	lat2Rad := degreeToRadians(lat2)
	lon1Rad := degreeToRadians(lon1)
	lon2Rad := degreeToRadians(lon2)

	dlon := lon2Rad - lon1Rad
	dlat := lat2Rad - lat1Rad

	a := math.Sin(dlat/2)*math.Sin(dlat/2) +
		math.Sin(dlon/2)*math.Sin(dlon/2)*
			math.Cos(lat1Rad)*math.Cos(lat2Rad)

	c := 2 * math.Atan2(math.Sqrt(a), math.Sqrt(1-a))

	return float64(earthRadius) * c

}
func degreeToRadians(degrees float64) float64 {
	return degrees * (math.Pi / 180.0)
}
