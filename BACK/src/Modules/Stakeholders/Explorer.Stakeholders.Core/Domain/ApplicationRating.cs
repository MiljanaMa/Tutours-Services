using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Stakeholders.Core.Domain;

public class ApplicationRating : Entity
{
    public ApplicationRating()
    {
    }

    public ApplicationRating(int rating, string comment, int userId, DateTime lastModified)
    {
        if (rating < 1 || rating > 5)
            throw new ArgumentException("Invalid rating value. Rating should be between 1 and 5.");

        Rating = rating;
        Comment = comment;
        UserId = userId;
        LastModified = lastModified;
        IsRated = true;
    }

    public int Rating { get; private set; }
    public string Comment { get; private set; }
    public int UserId { get; private set; }
    public DateTime LastModified { get; private set; }
    public bool IsRated { get; private set; }
}