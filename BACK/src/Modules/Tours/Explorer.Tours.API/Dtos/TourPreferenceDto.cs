namespace Explorer.Tours.API.Dtos;

public class TourPreferenceDto
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string Difficulty { get; set; }
    public string TransportType { get; set; }
    public List<string> Tags { get; set; }
}