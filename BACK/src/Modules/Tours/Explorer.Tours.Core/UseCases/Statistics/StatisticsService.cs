using AutoMapper;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Statistics;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.UseCases.Statistics
{
    public class StatisticsService : IStatisticsService
    {
        protected ITourProgressRepository _tourProgressRepository;

        public StatisticsService(ITourProgressRepository tourProgressRepository)
        {
            _tourProgressRepository = tourProgressRepository;
        }

        public Result<TourYearStatsDto> GetEncounterYearStatsByUser(int userId, int year)
        {
            TourYearStatsDto encounterYearStats = new TourYearStatsDto();
            encounterYearStats.Year = year;

            for (int month = 1; month <= 12; month++)
            {
                encounterYearStats.CompletedCountByMonths.Add(_tourProgressRepository.GetCompletedCountByUserAndMonth(userId, month, year));
            }

            return encounterYearStats;
        }
    }
}
