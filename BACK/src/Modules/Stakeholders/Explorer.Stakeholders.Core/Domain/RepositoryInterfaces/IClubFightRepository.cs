using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;

namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;

public interface IClubFightRepository : ICrudRepository<ClubFight>
{
    ClubFight GetWithClubs(int fightId);
    ClubFight Get(int fightId);
    List<ClubFight> GetTricky();
    ClubFight GetCurrentFightForOneOfTwoClubs(long clubId1, long clubId2);
    List<ClubFight> GetPassedUnfinishedFights();
    List<ClubFight> GetAllByClub(int clubId);
}