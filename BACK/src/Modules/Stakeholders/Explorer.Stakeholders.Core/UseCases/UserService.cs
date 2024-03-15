using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Internal;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using FluentResults;

namespace Explorer.Stakeholders.Core.UseCases;

public class UserService : CrudService<UserDto, User>, IUserService, IInternalUserService
{
    private readonly IMapper _mapper;
    private readonly ICrudRepository<User> _userRepository;
    private readonly IUserRepository _userDatabaseRepository;

    public UserService(ICrudRepository<User> repository, IMapper mapper, IUserRepository userDatabaseRepository) : base(repository, mapper)
    {
        _userRepository = repository;
        _mapper = mapper;
        _userDatabaseRepository = userDatabaseRepository;
    }

    public Result<List<UserDto>> GetMany(List<int> userIds)
    {
        List<UserDto> users = new();
        foreach (var id in userIds)
        {
            var user = _userRepository.Get(id);
            if (user != null)
            {
                var newUser = new UserDto
                {
                    Id = (int)user.Id,
                    Username = user.Username,
                    Password = user.Password,
                    Email = user.Email,
                    IsActive = user.IsActive,
                    IsBlocked = user.IsBlocked,
                    Role = (int)user.Role
                };
                users.Add(newUser);
            }
        }

        return users;
    }

    public Result<PagedResult<UserDto>> GetAllTourists(int page, int pageSize)
    {
        var result = _userDatabaseRepository.GetAllTourists(page, pageSize);
        return MapToDto(result);
    }

    public Result<UserDto> GetByToken(string token)
    {
        try
        {
            var result = _userDatabaseRepository.GetByToken(token);
            return MapToDto(result);
        }
        catch (KeyNotFoundException e)
        {
            return Result.Fail(FailureCode.NotFound).WithError(e.Message);
        }
    }
}