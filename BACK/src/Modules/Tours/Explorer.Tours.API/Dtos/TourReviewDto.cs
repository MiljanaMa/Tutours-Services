namespace Explorer.Tours.API.Dtos;

public class TourReviewDto
{
    public int Id { get; set; }
    public int Rating { get; set; }
    public long? UserId { get; set; }
    public string Comment { get; set; }

    public long TourId { get; set; }

    //public Tourist Tourist { get; set; }
    public DateTime VisitDate { get; set; }
    public DateTime RatingDate { get; set; }
    public List<string>? ImageLinks { get; set; }
}