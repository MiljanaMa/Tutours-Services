using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories;

public class PersonDatabaseRepository : CrudDatabaseRepository<Person, StakeholdersContext>, IPersonRepository
{
    private readonly StakeholdersContext _dbContext;

    public PersonDatabaseRepository(StakeholdersContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public bool Exists(long personId)
    {
        return DbContext.People.Any(person => person.Id == personId);
    }

    public Person GetFullProfile(long personId)
    {
        var person = _dbContext.People.Where(b => b.Id == personId)
            .Include("Following")
            .Include("Followers")
            .FirstOrDefault();
        //.IgnoreAutoIncludes();
        return person;
    }
    public Person GetFull(long personId)
    {
        var person = _dbContext.People.Where(b => b.Id == personId)
            .Include("Following")
            .Include("Followers")
            .Include(p => p.Club)
            .ThenInclude(c => c.Achievements)
            .FirstOrDefault();
        return person;
    }
}