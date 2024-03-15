namespace Explorer.Tours.API.Dtos;

public class TourProgressDto
{
    public long Id { get; set; }
    public long TouristPositionId { get; set; }
    public TouristPositionDto? TouristPosition { get; set; }
    public long TourId { get; set; }
    public TourDto? Tour { get; set; }
    public string Status { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime LastActivity { get; set; }
    public int CurrentKeyPoint { get; set; }
}