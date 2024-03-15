using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.API.Dtos
{
    public class EncounterCompletionDto
    {
        public long UserId { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public int EncounterId { get; set; } // to avoid too much data being sent, don't want to always include entire encounter
        public EncounterDto Encounter { get; set; }
        public int Xp { get; init; }
        public string Status { get; set; }
    }
}
