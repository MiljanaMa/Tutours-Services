using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using FluentResults;

namespace Explorer.Stakeholders.API.Public;

public interface IProfileService
{
    Result<AccountRegistrationDto> GetProfile(long userId);
    Result<PersonDto> GetPersonDto(long userId);
    Result<PagedResult<PersonDto>> GetFollowers(long userId);
    Result<PagedResult<PersonDto>> GetFollowing(long userId);
    Result<PagedResult<PersonDto>> GetUserNonFollowedProfiles(int page, int pageSize, long userId);
    Result<PersonDto> UpdateProfile(PersonDto updatedPerson);
    Result<PagedResult<PersonDto>> Follow(long followerId, long followedId);
    Result<PagedResult<PersonDto>> Unfollow(long followerId, long unfollowedId);
    Result<bool> CanTouristCreateEncounters(long touristId);
    Result<PersonDto> Get(long userId);
    Result<PersonDto> GetFull(long userId);
}