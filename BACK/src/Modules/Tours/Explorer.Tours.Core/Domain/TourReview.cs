using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Tours.Core.Domain;

public class TourReview : Entity
{
    public TourReview()
    {
    }

    public TourReview(int rating, string comment, DateTime visitDate, DateTime ratingDate, List<string> imageLinks,
        long userId, long tourId)
    {
        if (rating < 1 || rating > 5) throw new ArgumentException("Rating must be between 1 and 5");

        Rating = rating;
        Comment = comment;
        VisitDate = visitDate;
        RatingDate = ratingDate;
        ImageLinks = imageLinks;
        UserId = userId;
        TourId = tourId;
    }

    public int Rating { get; init; }
    public string Comment { get; init; }

    public long? UserId { get; init; }

    //public Tour Tour {  get; init; } 
    public long TourId { get; init; }
    public Tour Tour { get; init; }
    public DateTime VisitDate { get; init; }
    public DateTime RatingDate { get; init; }
    public List<string>? ImageLinks { get; init; }
}