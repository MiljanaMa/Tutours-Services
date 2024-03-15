namespace Explorer.Tours.API.Dtos;

public class TourDto1
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public int? Duration { get; set; }
    public double? Distance { get; set; }
    public int Difficulty { get; set; }
    public int TransportType { get; set; }
    public string Status { get; set; }
    public List<string>? Tags { get; set; }
    public DateTime StatusUpdateTime { get; set; }
    public List<KeypointDto>? Keypoints { get; set; }
    public List<TourReviewDto>? TourReviews { get; set; }
}