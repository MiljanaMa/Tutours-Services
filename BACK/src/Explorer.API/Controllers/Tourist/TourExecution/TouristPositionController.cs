using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.TourExecution;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace Explorer.API.Controllers.Tourist.TourExecution;

[Authorize(Policy = "touristPolicy")]
[Route("api/tourist/position")]
public class TouristPositionController : BaseApiController
{
    private readonly ITouristPositionService _touristPositionService;

    private static HttpClient sharedClient = new()
    {
        BaseAddress = new Uri(" http://localhost:8000/positions/"),
    };

    public TouristPositionController(ITouristPositionService touristPositionService)
    {
        _touristPositionService = touristPositionService;
    }

    /*
    [HttpGet]
    public ActionResult<TouristPositionDto> GetByUser()
    {
        var result = _touristPositionService.GetByUser(User.PersonId());
        return CreateResponse(result);
    }
    */

    
    [HttpGet]
    public async Task<ActionResult<TouristPositionDto>> GetByUser()
    {
        var nesto = User.PersonId();
        var httpResponse = await sharedClient.GetAsync($"getByUser/{User.PersonId()}");

        if (httpResponse.IsSuccessStatusCode)
        {
            var responseContent = await httpResponse.Content.ReadAsStringAsync();

            try
            {
                var position = JsonSerializer.Deserialize<TouristPositionDto>(responseContent);
                return Ok(position);
            }
            catch (JsonException ex)
            {
                return BadRequest("Error deserializing JSON response");
            }
        }
        else
        {
            return StatusCode((int)httpResponse.StatusCode, "Error while getting position");
        }
    }
    
    /*
    [HttpPost] 
    public ActionResult<TourPreferenceDto> Create([FromBody] TouristPositionDto touristPosition)
    {
        touristPosition.UserId = User.PersonId();
        var result = _touristPositionService.Create(touristPosition);
        return CreateResponse(result);
    }
    */
    
    
    [HttpPost]
    public async Task<ActionResult<TourPreferenceDto>> Create([FromBody] TouristPositionDto touristPosition)
    {
        touristPosition.UserId = User.PersonId();
        touristPosition.UpdatedAt = DateTime.UtcNow;
        var jsonContent = JsonSerializer.Serialize(touristPosition);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var httpResponse = await sharedClient.PostAsync("create", content);

        if (httpResponse.IsSuccessStatusCode)
        {
            var responseContent = await httpResponse.Content.ReadAsStringAsync();
            var createdPosition = JsonSerializer.Deserialize<KeypointDto>(responseContent);
            return Ok(createdPosition);
        }
        else
        {
            return StatusCode((int)httpResponse.StatusCode, "Error while creating tourist position");
        }
    }

    /*
    [HttpPut]
    public ActionResult<TouristPositionDto> Update([FromBody] TouristPositionDto touristPosition)
    {
        touristPosition.UserId = User.PersonId();
        var result = _touristPositionService.Update(touristPosition);
        return CreateResponse(result);
    }
    */

    [HttpPut]
    public async Task<ActionResult<TouristPositionDto>> Update([FromBody] TouristPositionDto touristPosition)
    {
        touristPosition.UserId = User.PersonId();
        touristPosition.UpdatedAt = DateTime.UtcNow;
        var jsonContent = JsonSerializer.Serialize(touristPosition);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var httpResponse = await sharedClient.PostAsync("update", content);

        if (httpResponse.IsSuccessStatusCode)
        {
            var responseContent = await httpResponse.Content.ReadAsStringAsync();
            var createdPosition = JsonSerializer.Deserialize<TouristPositionDto>(responseContent);

            return Ok(createdPosition);
        }
        else
        {
            return StatusCode((int)httpResponse.StatusCode, "Error while updating tourist position");
        }
    }
}