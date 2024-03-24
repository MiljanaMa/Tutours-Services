using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.API.Dtos
{
    public class EncounterCompletionDto_1
    {
            public long UserId { get; set; }
            public DateTime LastUpdatedAt { get; set; }
            public string EncounterId { get; set; }
            public int Xp { get; init; }
            public string Status { get; set; }
            public EncounterDto Encounter { get; set; }


    }
}
