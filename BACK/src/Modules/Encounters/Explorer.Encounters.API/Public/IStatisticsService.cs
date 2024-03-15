using Explorer.Encounters.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.API.Public
{
    public interface IStatisticsService
    {
        Result<EncounterStatsDto> GetEncounterStatsByUser(int userId);
        Result<EncounterYearStatsDto> GetEncounterYearStatsByUser(int userId, int year);
    }
}
