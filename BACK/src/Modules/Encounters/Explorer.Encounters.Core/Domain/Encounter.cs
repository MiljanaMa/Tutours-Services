using Explorer.BuildingBlocks.Core.Domain;
using Explorer.Encounters.Core.Domain.Enums;

namespace Explorer.Encounters.Core.Domain
{
    public class Encounter : Entity
    {
        public int UserId { get; init; }
        public String Name { get; init; }
        public String Description {  get; init; }
        public double Latitude {  get; private set; }
        public double Longitude { get; private set; }
        public int Xp {  get; init; }
        public EncounterStatus Status { get; init; }
        public EncounterType Type { get; init; }
        public double Range { get; init; }
        public string? Image {  get; init; }
        public double? ImageLatitude { get; private set; }
        public double? ImageLongitude { get; private set; }
        public int? PeopleCount { get; init; }
        public EncounterApprovalStatus ApprovalStatus { get; private set; }

        public Encounter() { }


        public Encounter(int userId, string name, string description, double latitude, double longitude, int xp, EncounterStatus status, EncounterType type, double range, string image, double imageLatitude, double imageLongitude, int peopleCount, EncounterApprovalStatus approvalStatus)
        {
            UserId = userId;
            Name = name;
            Description = description;
            Latitude = latitude;
            Longitude = longitude;
            Xp = xp;
            Status = status;
            Type = type;
            Range = range;
            Image = image;
            ImageLatitude = imageLatitude;
            ImageLongitude = imageLongitude;
            PeopleCount = peopleCount;
            ApprovalStatus = approvalStatus;
        }
        public void UpdateLocation(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public void UpdateApprovalStatus(EncounterApprovalStatus approvalStatus) 
        {
            ApprovalStatus = approvalStatus;
        }

    }
}
