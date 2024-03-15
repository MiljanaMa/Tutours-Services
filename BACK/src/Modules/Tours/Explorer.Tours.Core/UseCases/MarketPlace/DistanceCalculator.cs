namespace Explorer.Tours.Core.UseCases.MarketPlace;

public static class DistanceCalculator
{
    public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        const double earthRadius = 6371;

        var lat1Rad = DegreesToRadians(lat1);
        var lon1Rad = DegreesToRadians(lon1);
        var lat2Rad = DegreesToRadians(lat2);
        var lon2Rad = DegreesToRadians(lon2);

        var dLat = lat2Rad - lat1Rad;
        var dLon = lon2Rad - lon1Rad;

        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        var distance = earthRadius * c;

        return distance;
    }

    private static double DegreesToRadians(double degrees)
    {
        return degrees * (Math.PI / 180.0);
    }
}