using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.API.Dtos
{
    public class EncounterDto_1
    {
        public string Id { get; set; }
        public int UserId { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Xp { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public double Range { get; set; }
        public string? Image { get; set; }
        public double? ImageLatitude { get; set; }
        public double? ImageLongitude { get; set; }
        public int? PeopleCount { get; set; }
        public string ApprovalStatus { get; set; }

    }
}
