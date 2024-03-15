using Explorer.Stakeholders.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Internal
{
    public interface IInternalAchievementService
    {
        Result<AchievementDto> Get(int id);
        AchievementDto GetNoTracking(int id);
        AchievementDto getFightAchievement(ClubDto club);
        public AchievementDto getHiddenEncounterAchievement(int completedCount);
    }
}
