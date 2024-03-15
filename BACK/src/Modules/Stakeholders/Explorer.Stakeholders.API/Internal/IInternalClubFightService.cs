using Explorer.Stakeholders.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Internal
{
    public interface IInternalClubFightService
    {
        Result<ClubFightDto> Get(int id);
        Result<List<ClubFightDto>> GetTricky();
        Result<List<ClubFightDto>> GetPassedUnfinishedFights();
        Result<List<ClubFightDto>> UpdateMultiple(List<ClubFightDto> clubFights);
    }
}
