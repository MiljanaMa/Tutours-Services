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

    protected static HttpClient sharedClient = new()
    {
        BaseAddress = new Uri($"http://{Environment.GetEnvironmentVariable("TOUR_HOST") ?? "localhost"}:{Environment.GetEnvironmentVariable("TOUR_PORT") ?? "8000"}/positions/")
    };

    public TouristPositionController(ITouristPositionService touristPositionService)
    {
        _touristPositionService = touristPositionService;
    }


    
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