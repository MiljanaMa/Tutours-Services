using Explorer.BuildingBlocks.Core.Domain;
using Explorer.Tours.Core.Domain.Enum;

namespace Explorer.Tours.Core.Domain;

public class Tour : Entity
{
    public Tour()
    {
        Tags = new List<string>();
    }

    public Tour(int userId, string name, string description, double price, TourDifficulty? difficulty,
        TransportType? transportType,
        TourStatus? status, List<string> tags, int? duration, double? distance, DateTime statusUpdateTime)
    {
        UserId = userId;
        Name = name;
        Description = description;
        Price = price;
        Difficulty = difficulty;
        TransportType = transportType;
        Status = status;
        Tags = tags;
        Duration = duration;
        Distance = distance;
        StatusUpdateTime = statusUpdateTime;

        Validate();
    }

    public int UserId { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public double Price { get; init; }
    public int? Duration { get; init; }
    public double? Distance { get; init; }
    public TourDifficulty? Difficulty { get; init; }
    public TransportType? TransportType { get; init; }
    public TourStatus? Status { get; init; }
    public DateTime StatusUpdateTime { get; init; }
    public List<string>? Tags { get; init; }
    public ICollection<TourEquipment> TourEquipments { get; set; }
    public ICollection<Keypoint>? Keypoints { get; set; }
    public ICollection<TourReview>? TourReviews { get; set; }
    public ICollection<Bundle>? Bundles { get; set; }

    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(Description)) throw new ArgumentException("Invalid description.");
        if (Price < 0) throw new Exception("Invalid price.");
        if (string.IsNullOrEmpty(Name)) throw new ArgumentException("Invalid name");
        if (Distance < 0) throw new Exception("Invalid distance.");
        if (Duration < 0) throw new Exception("Invalid duration.");
    }
}