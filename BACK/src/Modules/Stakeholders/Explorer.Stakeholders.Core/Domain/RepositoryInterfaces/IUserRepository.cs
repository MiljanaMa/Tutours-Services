using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;


namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;

public interface IUserRepository
{
    bool Exists(string username);
    User? GetActiveByName(string username);
    User Create(User user);
    long GetPersonId(long userId);
    User? GetActiveById(long id);
    PagedResult<User> GetAllTourists(int page, int pageSize);
    public User GetByToken(string token);
}