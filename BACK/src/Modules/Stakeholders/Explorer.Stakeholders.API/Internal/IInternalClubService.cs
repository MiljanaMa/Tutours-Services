using Explorer.Stakeholders.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Internal
{
    public interface IInternalClubService
    {
        Result<ClubDto> GetWithMembers(int clubId);
        Result<ClubDto> AddAchievement(long clubId, long achievementId);
    }
}
