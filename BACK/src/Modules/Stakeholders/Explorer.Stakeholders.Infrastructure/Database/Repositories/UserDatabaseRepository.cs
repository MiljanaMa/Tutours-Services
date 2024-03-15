using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories;

public class UserDatabaseRepository : IUserRepository
{
    private readonly StakeholdersContext _dbContext;

    public UserDatabaseRepository(StakeholdersContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool Exists(string username)
    {
        return _dbContext.Users.Any(user => user.Username == username);
    }

    public User? GetActiveByName(string username)
    {
        return _dbContext.Users.FirstOrDefault(user => user.Username == username && user.IsActive);
    }

    public User Create(User user)
    {
        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();
        return user;
    }

    public long GetPersonId(long userId)
    {
        var person = _dbContext.People.FirstOrDefault(i => i.UserId == userId);
        if (person == null) throw new KeyNotFoundException("Not found.");
        return person.Id;
    }

    public User? GetActiveById(long id)
    {
        return _dbContext.Users.FirstOrDefault(user => user.Id == id && user.IsActive);
    }

    public PagedResult<User> GetAllTourists(int page, int pageSize)
    {
        var task = _dbContext.Users.Where(u => u.Role == UserRole.Tourist).GetPagedById(page, pageSize);
        task.Wait();
        return task.Result;
    }
    
    public User GetByToken(string token)
    {
        var user = _dbContext.Users.AsNoTracking().FirstOrDefault(user => user.VerificationToken == token);
        return user;
        
    }
}