using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Explorer.Tours.Core.Domain;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories;

public class ClubRepository : CrudDatabaseRepository<Club, StakeholdersContext>, IClubRepository
{
    private readonly DbSet<Club> _dbSet;
    
    public ClubRepository(StakeholdersContext dbContext) : base(dbContext)
    {
        _dbSet = dbContext.Clubs;
    }

    public Club GetUntracked(long id)
    {
        return _dbSet.AsNoTracking().FirstOrDefault(c => c.Id == id);
    }

    public Club GetWithMembers(long id)
    {
        return _dbSet.AsNoTracking().Include(c => c.Owner).Include(c => c.Members).Include(c => c.Achievements).FirstOrDefault(c => c.Id == id);
    }
    public PagedResult<Club> GetAll(int page, int pageSize)
    {
        var task = DbContext.Clubs
                .Include("Members")
                .Include("Achievements")
                .OrderByDescending(c => c.FightsWon)
                .GetPaged(page, pageSize);
        task.Wait();
        return task.Result;
    }
    public Club Get(long id)
    {
        var club = DbContext.Clubs.Where(c => c.Id == id)
                .Include("Members")
                .Include("Achievements")
                .FirstOrDefault();
        return club;
    }
}