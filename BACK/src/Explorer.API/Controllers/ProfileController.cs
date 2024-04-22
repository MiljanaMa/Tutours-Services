using Explorer.Blog.API.Dtos;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Infrastructure.Authentication;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Explorer.API.Controllers;

[Authorize(Policy = "userPolicy")]
[Route("api/profile")]
public class ProfileController : BaseApiController
{
    private readonly IProfileService _profileService;
    private readonly IUserService _userService;

    protected static HttpClient httpClient = new()
    {
        BaseAddress = new Uri($"http://localhost:8095/followers/") //change to profile or not? followers it is then, because of followers microservice
    };

    public ProfileController(IProfileService profileService, IUserService userService)
    {
        _profileService = profileService;
        _userService = userService;
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

    [HttpGet("get-recommendations")]
    public async Task<ActionResult<PagedResult<UserDto>>> GetRecommendations()
    {
        int id = User.PersonId();
        HttpResponseMessage response = await httpClient.GetAsync("get-recommendations/" + id);

        if (response.IsSuccessStatusCode)
        {
            string json = response.Content.ReadAsStringAsync().Result;
            List<int> userIds = JsonSerializer.Deserialize<List<int>>(json);
            List<UserDto> users = new List<UserDto>();
            if (userIds.Count > 0)
            {
                foreach (int u in userIds)
                {
                    users.Add(_userService.Get(u).Value);
                }
            }

            PagedResult<UserDto> result;
            if (users.Count > 0)
            {
                result = new PagedResult<UserDto>(users, users.Count);
            }
            else
            {
                result = new PagedResult<UserDto>(new List<UserDto>(), 0);
            }

            return Ok(result);
        }
        return BadRequest($"Error: {response.StatusCode} - {response.ReasonPhrase}");
    }

    [HttpGet("followers")]
    public async Task<ActionResult<PagedResult<UserDto>>> GetFollowers()
    {
        int id = User.PersonId();
        HttpResponseMessage response = await httpClient.GetAsync("get-followers/" + id);

        if (response.IsSuccessStatusCode)
        {
            string json = response.Content.ReadAsStringAsync().Result;
            List<int> userIds = JsonSerializer.Deserialize<List<int>>(json);
            List<UserDto> users = new List<UserDto>();
            if (userIds.Count > 0)
            {
                foreach (int u in userIds)
                {
                    users.Add(_userService.Get(u).Value);
                }
            }

            PagedResult<UserDto> result;
            if (users.Count > 0)
            {
                result = new PagedResult<UserDto>(users, users.Count);
            }
            else
            {
                result = new PagedResult<UserDto>(new List<UserDto>(), 0);
            }

            return Ok(result);
        }
        return BadRequest($"Error: {response.StatusCode} - {response.ReasonPhrase}");
    }

    [HttpGet("followings")]
    public async Task<ActionResult<PagedResult<UserDto>>> GetFollowing()
    {
        int id = User.PersonId();
        HttpResponseMessage response = await httpClient.GetAsync("get-followings/" + id);

        if (response.IsSuccessStatusCode)
        {
            string json = response.Content.ReadAsStringAsync().Result;
            List<int> userIds = JsonSerializer.Deserialize<List<int>>(json);
            List<UserDto> users = new List<UserDto>();
            if (userIds.Count > 0)
            {
                foreach (int u in userIds)
                {
                    users.Add(_userService.Get(u).Value);
                }
            }

            PagedResult<UserDto> result;
            if (users.Count > 0)
            {
                result = new PagedResult<UserDto>(users, users.Count);
            }
            else
            {
                result = new PagedResult<UserDto>(new List<UserDto>(), 0);
            }

            return Ok(result);
        }
        return BadRequest($"Error: {response.StatusCode} - {response.ReasonPhrase}");
    }

    [HttpPut("{id:int}")]
    public ActionResult<PersonDto> Update(int id, [FromBody] PersonDto updatedPerson)
    {
        updatedPerson.Id = id;

        var result = _profileService.UpdateProfile(updatedPerson);
        return CreateResponse(result);
    }

    [HttpPost("follow/{id}")]
    public async Task<IActionResult> Follow(int id)
    {
        if (_userService.Get(id).Value != null)
        {
            HttpResponseMessage response = await httpClient.PostAsync("follow/" + User.PersonId() + "/" + id, null);

            if (response.IsSuccessStatusCode)
            {
                return Ok();
            }
            return BadRequest($"Error: {response.StatusCode} - {response.ReasonPhrase}");
        }
        return NotFound();
    }

    [HttpDelete("unfollow/{id}")]
    public async Task<IActionResult> Unfollow(int id)
    {
        if (_userService.Get(id).Value != null)
        {
            HttpResponseMessage response = await httpClient.PostAsync("unfollow/" + User.PersonId() + "/" + id, null);

            if (response.IsSuccessStatusCode)
            {
                return Ok();
            }
            return BadRequest($"Error: {response.StatusCode} - {response.ReasonPhrase}");
        }
        return NotFound();
    }

    [HttpGet("canCreateEncounters")]
    public ActionResult<bool> CanTouristCreateEncounters()
    {
        var result = _profileService.CanTouristCreateEncounters(ClaimsPrincipalExtensions.PersonId(User));
        return CreateResponse(result);
    }

}