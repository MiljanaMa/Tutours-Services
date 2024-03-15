using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories;

public class ClubFightRepository : CrudDatabaseRepository<ClubFight, StakeholdersContext>, IClubFightRepository
{
    private DbSet<ClubFight> _dbSet;
    
    public ClubFightRepository(StakeholdersContext dbContext) : base(dbContext)
    {
        _dbSet = dbContext.ClubFights;
    }

    public ClubFight GetWithClubs(int fightId)
    {
        return _dbSet.AsNoTracking().Include(cf => cf.Club1).Include(cf => cf.Club2).FirstOrDefault(cf => cf.Id == fightId);
    }
    
    public ClubFight Get(int fightId)
    {
        return _dbSet.FirstOrDefault(cf => cf.Id == fightId);
    }

    public ClubFight GetCurrentFightForOneOfTwoClubs(long clubId1, long clubId2)
    {
        return _dbSet.AsNoTracking().FirstOrDefault(cf => cf.IsInProgress && (cf.Club1Id == clubId1 || cf.Club2Id == clubId1 || cf.Club1Id == clubId2 || cf.Club2Id == clubId2));
    }

    public List<ClubFight> GetPassedUnfinishedFights()
    {
        return _dbSet.AsNoTracking().Where(cf => cf.IsInProgress && cf.EndOfFight <= DateTime.UtcNow)
            .Include(cf => cf.Club1)
            .Include(cf => cf.Club1.Members)
            .Include(cf => cf.Club1.Achievements)
            .Include(cf => cf.Club2)
            .Include(cf => cf.Club2.Members)
            .Include(cf => cf.Club2.Achievements).ToList();
    }    
    public List<ClubFight> GetTricky()
    {
        return _dbSet.AsNoTracking().Where(cf => cf.IsInProgress)
            .Include(cf => cf.Club1)
            .Include(cf => cf.Club1.Members)
            .Include(cf => cf.Club1.Achievements)
            .Include(cf => cf.Club2)
            .Include(cf => cf.Club2.Members)
            .Include(cf => cf.Club2.Achievements).ToList();
    }

    public List<ClubFight> GetAllByClub(int clubId)
    {
        return _dbSet.Where(cf => cf.Club1Id == clubId || cf.Club2Id == clubId)
            .Include(cf => cf.Club1)
            .Include(cf => cf.Club2)
            .OrderByDescending(cf => cf.EndOfFight).ToList();
    }
}