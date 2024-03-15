using Explorer.BuildingBlocks.Core.Domain;
using Explorer.Encounters.Core.Domain.Enums;

namespace Explorer.Encounters.Core.Domain
{
    public class EncounterCompletion : Entity
    {
        public long UserId { get; init; }
        public DateTime LastUpdatedAt { get; private set; }
        public long EncounterId { get; }
        public Encounter Encounter { get; }
        public int Xp { get; init; }
        public EncounterCompletionStatus Status { get; private set; }

        public bool IsStarted => Status == EncounterCompletionStatus.STARTED;
        public bool IsFinished => Status == EncounterCompletionStatus.FAILED || Status == EncounterCompletionStatus.COMPLETED;

        public EncounterCompletion() { }

        public EncounterCompletion(long userId, long encounterId, int xp, EncounterCompletionStatus status)
        {
            UserId = userId;
            EncounterId = encounterId;
            LastUpdatedAt = DateTime.UtcNow;
            Xp = xp;
            Status = status;
        }

        public void UpdateStatus(EncounterCompletionStatus status)
        {
            Status = status;
        }

        public void Complete()
        {
            Status = EncounterCompletionStatus.COMPLETED;
            LastUpdatedAt = DateTime.UtcNow;
        }

        public void Reset()
        {
            Status = EncounterCompletionStatus.STARTED;
            LastUpdatedAt = DateTime.UtcNow;
        }

        public void Progress()
        {
            Status = EncounterCompletionStatus.PROGRESSING;
            LastUpdatedAt = DateTime.UtcNow;
        }
    }
}
