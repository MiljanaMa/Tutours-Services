using AutoMapper;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Explorer.Stakeholders.Core.Domain;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain.Enums;
using Explorer.Stakeholders.API.Internal;

namespace Explorer.Stakeholders.Core.UseCases
{
    public class AchievementService : CrudService<AchievementDto, Achievement>, IAchievementService, IInternalAchievementService
    {
        private readonly IAchievementRepository _achievementRepository;

        public AchievementService(IAchievementRepository repository, IMapper mapper) : base(repository, mapper)
        {
            _achievementRepository = repository;
        }

        public AchievementDto GetNoTracking(int id)
        {
            return MapToDto(_achievementRepository.GetNoTracking(id));
        }

        public AchievementDto getFightAchievement(ClubDto club)
        {
            if (!club.Achievements.Any(a => (int)a.Type == (int)AchievementType.FIGHT_2))
                return club.FightsWon > 5 ? MapToDto(_achievementRepository.GetByType(AchievementType.FIGHT_2)) : null;
            else if (!club.Achievements.Any(a => (int)a.Type == (int)AchievementType.FIGHT_1))
                return club.FightsWon > 10 ? MapToDto(_achievementRepository.GetByType(AchievementType.FIGHT_1)) : null;
            else
                return null;
        }
        public AchievementDto getHiddenEncounterAchievement(int completedCount)
        {
            if (completedCount >= 1)
                return MapToDto(_achievementRepository.GetByType(AchievementType.HIDDEN_ENCOUNTER));
            return null;
        }

    }
}
