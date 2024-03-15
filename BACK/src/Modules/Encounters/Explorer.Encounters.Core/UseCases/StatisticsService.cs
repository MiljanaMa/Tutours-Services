using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public;
using Explorer.Encounters.Core.Domain;
using Explorer.Encounters.Core.Domain.RepositoryInterfaces;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.Core.UseCases
{
    public class StatisticsService : IStatisticsService
    {
        protected IEncounterCompletionRepository _encounterCompletionRepository;

        public StatisticsService(IEncounterCompletionRepository encoutnerCompletionRepository)
        { 
            _encounterCompletionRepository = encoutnerCompletionRepository;
        }

        public Result<EncounterStatsDto> GetEncounterStatsByUser(int userId) 
        { 
            EncounterStatsDto encounterStats = new EncounterStatsDto();
            encounterStats.CompletedCount = _encounterCompletionRepository.GetCompletedCountByUser(userId);
            encounterStats.FailedCount = _encounterCompletionRepository.GetFailedCountByUser(userId);
            return encounterStats;
        }

        public Result<EncounterYearStatsDto> GetEncounterYearStatsByUser(int userId, int year)
        {
            EncounterYearStatsDto encounterYearStats = new EncounterYearStatsDto();
            encounterYearStats.Year = year;
            for(int month = 1; month <= 12; month++)
            {
                encounterYearStats.CompletedCountByMonths.Add(_encounterCompletionRepository.GetCompletedCountByUserAndMonth(userId, month, year));
                encounterYearStats.FailedCountByMonths.Add(_encounterCompletionRepository.GetFailedCountByUserAndMonth(userId, month, year));
            }
           
            return encounterYearStats;
        }
    }
}
