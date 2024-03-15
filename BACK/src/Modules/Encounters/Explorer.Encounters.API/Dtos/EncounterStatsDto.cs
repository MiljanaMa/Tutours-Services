using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.API.Dtos
{
    public class EncounterStatsDto
    {
        public int CompletedCount { get; set; }
        public int FailedCount { get; set; }
    }
}
