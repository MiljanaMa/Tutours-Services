using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.TourAuthoring;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using FluentResults;

namespace Explorer.API.Controllers.Author;

[Authorize(Policy = "personPolicy")]
[Route("api/author/keypoints")]
public class KeypointController : BaseApiController
{
    private readonly IKeypointService _keypointService;

    private static HttpClient sharedClient = new()
    {
        BaseAddress = new Uri(" http://localhost:8000/keypoints/"),
    };

    public KeypointController(IKeypointService keypointService)
    {
        _keypointService = keypointService;
    }

    [HttpGet]
    public ActionResult<PagedResult<KeypointDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
    {
        var result = _keypointService.GetPaged(page, pageSize);
        return CreateResponse(result);
    }

    /*
    [HttpGet("tour/{tourId:int}")]
    public ActionResult<PagedResult<KeypointDto>> GetByTour([FromQuery] int page, [FromQuery] int pageSize,
        [FromRoute] int tourId)
    {
        var result = _keypointService.GetByTourId(page, pageSize, tourId);
        return CreateResponse(result);
    }
    */

    [HttpGet("tour/{tourId:int}")]
    public async Task<ActionResult<PagedResult<KeypointDto>>> GetByTour([FromQuery] int page, [FromQuery] int pageSize, [FromRoute] int tourId)
    {
        var httpResponse = await sharedClient.GetAsync($"getByTour/{tourId}");

        if (httpResponse.IsSuccessStatusCode)
        {
            var responseContent = await httpResponse.Content.ReadAsStringAsync();

            try
            {
                var keypoints = JsonSerializer.Deserialize<List<KeypointDto>>(responseContent);            
                var pagedResult = new PagedResult<KeypointDto>(keypoints.ToList(), keypoints.Count);
                return pagedResult;
            }
            catch (JsonException ex)
            {
                return BadRequest("Error deserializing JSON response");
            }
        }
        else
        {
            return StatusCode((int)httpResponse.StatusCode, "Error while getting keypoint");
        }
    }

    /*

    [HttpPost]
    public ActionResult<KeypointDto> Create([FromBody] KeypointDto keypoint)
    {
        var result = _keypointService.Create(keypoint);
        return CreateResponse(result);
    }
    */

    [HttpPost]
    public async Task<ActionResult<KeypointDto>> Create([FromBody] KeypointDto keypoint)
    {
        var jsonContent = JsonSerializer.Serialize(keypoint);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var httpResponse = await sharedClient.PostAsync("create", content);

        if (httpResponse.IsSuccessStatusCode)
        {
            var responseContent = await httpResponse.Content.ReadAsStringAsync();
            var createdKeypoint = JsonSerializer.Deserialize<KeypointDto>(responseContent);
            return Ok(createdKeypoint);
        }
        else
        {
            return StatusCode((int)httpResponse.StatusCode, "Error while creating keypoint");
        }
    }



    [HttpPost("/multiple")]
    public ActionResult<KeypointDto> CreateMultiple([FromBody] List<KeypointDto> keypoints)
    {
        var result = _keypointService.CreateMultiple(keypoints);
        return CreateResponse(result);
    }

    /*
    [HttpPut("{id:int}")]
    public ActionResult<KeypointDto> Update([FromBody] KeypointDto keypoint)
    {
        var result = _keypointService.Update(keypoint);
        return CreateResponse(result);
    }
    */

    [HttpPut]
    public async Task<ActionResult<KeypointDto>> Update([FromBody] KeypointDto keypoint)
    {
        var jsonContent = JsonSerializer.Serialize(keypoint);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var httpResponse = await sharedClient.PostAsync("update", content);

        if (httpResponse.IsSuccessStatusCode)
        {
            var responseContent = await httpResponse.Content.ReadAsStringAsync();
            var createdKeypoint = JsonSerializer.Deserialize<KeypointDto>(responseContent);

            return Ok(createdKeypoint);
        }
        else
        {
            return StatusCode((int)httpResponse.StatusCode, "Error while updating keypoint");
        }
    }


    /*
    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var result = _keypointService.Delete(id);
        return CreateResponse(result);
    }
    */


    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var httpResponse = await sharedClient.DeleteAsync($"delete/{id}");

        if (httpResponse.IsSuccessStatusCode)
        {
            return CreateResponse(Result.Ok());
        }
        else
        {
            return StatusCode((int)httpResponse.StatusCode, "Error while deleting keypoint");
        }
    }
}