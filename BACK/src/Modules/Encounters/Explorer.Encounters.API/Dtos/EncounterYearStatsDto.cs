using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.API.Dtos
{
    public class EncounterYearStatsDto
    {
        public int Year { get; set; }
        public List<int> CompletedCountByMonths { get; set; }
        public List<int> FailedCountByMonths { get; set; }

        public EncounterYearStatsDto() 
        {
            CompletedCountByMonths = new List<int>();   
            FailedCountByMonths = new List<int>();
        }
    }
}
