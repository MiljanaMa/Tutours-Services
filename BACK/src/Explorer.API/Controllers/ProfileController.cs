using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Infrastructure.Authentication;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers;

[Authorize(Policy = "userPolicy")]
[Route("api/profile")]
public class ProfileController : BaseApiController
{
    private readonly IProfileService _profileService;

    public ProfileController(IProfileService profileService)
    {
        _profileService = profileService;
    }

    [HttpGet("{userId:int}")]
    public ActionResult<AccountRegistrationDto> GetStakeholderProfile(long userId)
    {
        var result = _profileService.GetProfile(userId);
        return CreateResponse(result);
    }
    
    [HttpGet("zelimdaumrem/{userId:int}")]
    public ActionResult<AccountRegistrationDto> GetPersonDto(long userId)
    {
        var result = _profileService.GetPersonDto(userId);
        return CreateResponse(result);
    }

    [HttpGet("not-followed")]
    public ActionResult<PagedResult<PersonDto>> GetNonFollowedProfiles([FromQuery] int page, [FromQuery] int pageSize)
    {
        var result = _profileService.GetUserNonFollowedProfiles(page, pageSize, User.PersonId());
        return CreateResponse(result);
    }

    [HttpGet("followers")]
    public ActionResult<PagedResult<PersonDto>> GetFollowers()
    {
        var result = _profileService.GetFollowers(User.PersonId());
        return CreateResponse(result);
    }

    [HttpGet("following")]
    public ActionResult<PagedResult<PersonDto>> GetFollowing()
    {
        var result = _profileService.GetFollowing(User.PersonId());
        return CreateResponse(result);
    }

    [HttpPut("{id:int}")]
    public ActionResult<PersonDto> Update(int id, [FromBody] PersonDto updatedPerson)
    {
        updatedPerson.Id = id;

        var result = _profileService.UpdateProfile(updatedPerson);
        return CreateResponse(result);
    }

    [HttpPut("follow")]
    public ActionResult<PagedResult<PersonDto>> Follow([FromBody] long followedId)
    {
        try
        {
            var result = _profileService.Follow(User.PersonId(), followedId);
            return CreateResponse(result);
        }
        catch (ArgumentException e)
        {
            return CreateResponse(Result.Fail(FailureCode.InvalidArgument).WithError(e.Message));
        }
    }

    [HttpPut("unfollow")]
    public ActionResult<PagedResult<PersonDto>> Unfollow([FromBody] long unfollowedId)
    {
        try
        {
            var result = _profileService.Unfollow(User.PersonId(), unfollowedId);
            return CreateResponse(result);
        }
        catch (ArgumentException e)
        {
            return CreateResponse(Result.Fail(FailureCode.InvalidArgument).WithError(e.Message));
        }
    }

    [HttpGet("canCreateEncounters")]
    public ActionResult<bool> CanTouristCreateEncounters()
    {
        var result = _profileService.CanTouristCreateEncounters(ClaimsPrincipalExtensions.PersonId(User));
        return CreateResponse(result);
    }

}