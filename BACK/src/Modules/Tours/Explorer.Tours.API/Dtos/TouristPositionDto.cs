namespace Explorer.Tours.API.Dtos;

public class TouristPositionDto
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime UpdatedAt { get; set; }
}