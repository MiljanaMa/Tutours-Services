using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text.Json;
using Explorer.Blog.API.Dtos;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Encounters.API.Dtos;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.UseCases;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist;

[Authorize(Policy = "touristPolicy")]
[Route("api/followers")]
public class FollowerController: BaseApiController
{

    private readonly IUserService _userService; 
    protected static HttpClient httpClient = new()
    {
        BaseAddress = new Uri($"http://localhost:8095/followers/")
    };
    public FollowerController(IUserService userService)
    {
        _userService = userService; 
    }
    
    [HttpGet("get-recommendations")]
    public async Task<ActionResult<PagedResult<BlogDto>>> GetRecommendations()
    {
        int id = User.PersonId();
        HttpResponseMessage response = await httpClient.GetAsync("get-recommendations/"+id);

       
        if(response.IsSuccessStatusCode){
            string json = response.Content.ReadAsStringAsync().Result;
            List<int> userIds = JsonSerializer.Deserialize<List<int>>(json);
            List<UserDto> users = new List<UserDto>();
            if (userIds.Count > 0)
            {
                foreach (int u in userIds)
                {
                    users.Add( _userService.Get(u).Value); 
                }
            }

            PagedResult<UserDto> result; 
            if (users.Count >0)
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
    
}