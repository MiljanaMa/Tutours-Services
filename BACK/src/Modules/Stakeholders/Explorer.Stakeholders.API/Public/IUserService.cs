using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using FluentResults;

namespace Explorer.Stakeholders.API.Public;

public interface IUserService
{
    Result<PagedResult<UserDto>> GetPaged(int page, int pageSize);
    Result<UserDto> Get(int id);
    Result<UserDto> Create(UserDto user);
    Result<UserDto> Update(UserDto user);
    Result Delete(int id);
    Result<PagedResult<UserDto>> GetAllTourists(int page, int pageSize);
    public Result<UserDto> GetByToken(string token);
}