using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Dtos
{
    public class TourYearStatsDto
    {
        public int Year {  get; set; }
        public List<int> CompletedCountByMonths { get; set; }

        public TourYearStatsDto()
        {
            CompletedCountByMonths = new List<int>();
        }
    }
}
