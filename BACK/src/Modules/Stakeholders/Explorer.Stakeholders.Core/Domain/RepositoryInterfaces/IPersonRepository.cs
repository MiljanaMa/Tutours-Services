using Explorer.BuildingBlocks.Core.UseCases;

namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;

public interface IPersonRepository : ICrudRepository<Person>
{
    Person GetFullProfile(long personId);
    bool Exists(long personId);
    Person GetFull(long personId);
}