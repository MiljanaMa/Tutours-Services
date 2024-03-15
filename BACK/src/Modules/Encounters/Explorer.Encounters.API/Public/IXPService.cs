using Explorer.Encounters.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.API.Public
{
    public interface IXPService
    {
        Result<ClubFightXPInfoDto> GetClubFightXPInfo(int clubFightId);
        void UpdateFights(bool tricky);
    }
}
