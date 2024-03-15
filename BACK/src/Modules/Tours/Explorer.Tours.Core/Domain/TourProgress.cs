using Explorer.BuildingBlocks.Core.Domain;
using Explorer.Tours.Core.Domain.Enum;

namespace Explorer.Tours.Core.Domain;

public class TourProgress : Entity
{
    public long TouristPositionId { get; init; }
    public TouristPosition TouristPosition { get; set; }
    public long TourId { get; init; }
    public Tour Tour { get; set; }
    public TourProgressStatus Status { get; private set; }
    public DateTime StartTime { get; init; }
    public DateTime LastActivity { get; private set; }
    public int CurrentKeyPoint { get; private set; }

    public bool IsInProgress => Status == TourProgressStatus.IN_PROGRESS;
    public TourProgress()
    {
    }

    public TourProgress(long touristPositionId, long tourId)
    {
        TouristPositionId = touristPositionId;
        TourId = tourId;
        Status = TourProgressStatus.IN_PROGRESS;
        StartTime = DateTime.UtcNow;
        LastActivity = DateTime.UtcNow;
        CurrentKeyPoint = 1;
    }

    public void UpdateActivityTime()
    {
        LastActivity = DateTime.UtcNow;
    }

    public void Abandon()
    {
        Status = TourProgressStatus.ABANDONED;
        LastActivity = DateTime.UtcNow;
    }

    public void Complete()
    {
        Status = TourProgressStatus.COMPLETED;
        LastActivity = DateTime.UtcNow;
    }

    public void MoveToNextKeypoint(int nextKeypoint)
    {
        CurrentKeyPoint = nextKeypoint;
        LastActivity = DateTime.UtcNow;
    }
}