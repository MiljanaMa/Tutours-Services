using Explorer.Stakeholders.API.Dtos;
using FluentResults;

namespace Explorer.Stakeholders.API.Internal;

public interface IInternalUserService
{
    Result<List<UserDto>> GetMany(List<int> userIds);
    Result<UserDto> Get(int id);
}