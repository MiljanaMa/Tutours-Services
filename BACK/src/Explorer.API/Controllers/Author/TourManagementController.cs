using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.TourAuthoring;
using Explorer.Tours.Core.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Http;
using System;
using System.Runtime;

namespace Explorer.API.Controllers.Author;

[Authorize(Policy = "userPolicy")]
[Route("api/author/tours/")]
public class TourManagementController : BaseApiController
{
    private readonly ITourService _tourService;

    protected static HttpClient httpClient = new()
    {
        BaseAddress = new Uri($"http://{Environment.GetEnvironmentVariable("TOUR_HOST") ?? "localhost"}:{Environment.GetEnvironmentVariable("TOUR_PORT") ?? "8000"}/tours/")
    };

    public TourManagementController(ITourService tourService)
    {
        _tourService = tourService;
    }

    [HttpGet]
    [Authorize(Roles = "author, tourist")]
    public async Task<ActionResult<PagedResult<TourDto>>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
    {
        var httpResponse = await httpClient.GetAsync($"?page={page}&pageSize={pageSize}");

        if (httpResponse.IsSuccessStatusCode)
        {
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                var response = await httpResponse.Content.ReadFromJsonAsync<List<TourDto>>();

                return Ok(new PagedResult<TourDto>(response, response.Count));
            }

            return Ok();

        }
        else
        {
            return new ContentResult
            {
                StatusCode = (int)httpResponse.StatusCode,
                Content = await httpResponse.Content.ReadAsStringAsync(),
                ContentType = "text/plain"
            };
        }
    }
    
    [AllowAnonymous]
    [HttpGet("{tourId:int}")]
    [Authorize(Roles = "author")]
    public async Task<ActionResult<TourDto>> GetById([FromRoute] int tourId)
    {
        var httpResponse = await httpClient.GetAsync($"{tourId}");

        if (httpResponse.IsSuccessStatusCode)
        {
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                var response = await httpResponse.Content.ReadFromJsonAsync<TourDto>();

                return Ok(response);
            }

            return Ok();

        }
        else
        {
            return new ContentResult
            {
                StatusCode = (int)httpResponse.StatusCode,
                Content = await httpResponse.Content.ReadAsStringAsync(),
                ContentType = "text/plain"
            };
        }
    }

    [HttpPost]
    [Authorize(Roles = "author")]
    public async Task<ActionResult<TourDto>> Create([FromBody] TourDto tour)
    {
        tour.UserId = User.PersonId();

        string tourJson = JsonSerializer.Serialize(tour);
        using StringContent jsonContent = new StringContent(
            tourJson,
            Encoding.UTF8,
            "application/json"
        );

        var httpResponse = await httpClient.PostAsync(
        "create",
        jsonContent);

        if (httpResponse.IsSuccessStatusCode)
        {
            if (httpResponse.StatusCode == HttpStatusCode.Created)
            {
                var response = await httpResponse.Content.ReadFromJsonAsync<TourDto>();

                return Ok(response);
            }

            return Ok();

        }
        else
        {
            return new ContentResult
            {
                StatusCode = (int)httpResponse.StatusCode,
                Content = await httpResponse.Content.ReadAsStringAsync(),
                ContentType = "text/plain"
            };
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "author,tourist")]
    public async Task<ActionResult<TourDto>> Update([FromBody] TourDto tour)
    {
        tour.UserId = User.PersonId();
        string tourJson = JsonSerializer.Serialize(tour);
        using StringContent jsonContent = new StringContent(
            tourJson,
            Encoding.UTF8,
            "application/json"
        );

        var httpResponse = await httpClient.PostAsync(
        "update",
        jsonContent);

        if (httpResponse.IsSuccessStatusCode)
        {
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                var response = await httpResponse.Content.ReadFromJsonAsync<TourDto>();

                return Ok(response);
            }

            return Ok();

        }
        else
        {
            return new ContentResult
            {
                StatusCode = (int)httpResponse.StatusCode,
                Content = await httpResponse.Content.ReadAsStringAsync(),
                ContentType = "text/plain"
            };
        }
    }
    
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "author")]
    public async Task<ActionResult> Delete(int id)
    {
        using HttpResponseMessage httpResponse = await httpClient.DeleteAsync($"delete/{id}");

        if (httpResponse.IsSuccessStatusCode)
        {
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                var response = await httpResponse.Content.ReadAsStringAsync();

                return Ok(response);
            }

            return Ok();

        }
        else
        {
            return new ContentResult
            {
                StatusCode = (int)httpResponse.StatusCode,
                Content = await httpResponse.Content.ReadAsStringAsync(),
                ContentType = "text/plain"
            };
        }
    }

    [HttpGet("author")]
    [Authorize(Roles = "author")]
    public async Task<ActionResult<PagedResult<TourDto>>> GetByAuthor([FromQuery] int page, [FromQuery] int pageSize)
    {
        var authorId = User.PersonId();
        var httpResponse = await httpClient.GetAsync($"author/{authorId}");

        if (httpResponse.IsSuccessStatusCode)
        {
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                var response = await httpResponse.Content.ReadFromJsonAsync<List<TourDto>>();

                return Ok(new PagedResult<TourDto>(response, response.Count));
            }

            return Ok();

        }
        else
        {
            return new ContentResult
            {
                StatusCode = (int)httpResponse.StatusCode,
                Content = await httpResponse.Content.ReadAsStringAsync(),
                ContentType = "text/plain"
            };
        }
    }

    [AllowAnonymous]
    [HttpPut("disable/{id:int}")]
    public ActionResult<TourDto> Disable([FromBody] TourDto tour)
    {
        if (User.IsInRole("administrator"))
        {
            tour.UserId = User.PersonId();
            var result = _tourService.Update(tour);
            return CreateResponse(result);
        }
        return null;
    }
    
    [HttpPost("custom")]
    public ActionResult<TourDto> CreateCustomTour([FromBody] TourDto tourDto)
    {   
        tourDto.UserId = ClaimsPrincipalExtensions.PersonId(User);
        var result = _tourService.CreateCustom(tourDto);
        return CreateResponse(result);
    }

    [HttpPost("campaign")]
    public ActionResult<TourDto> CreateCampaignTour([FromBody] TourDto tourDto) 
    {
        tourDto.UserId = ClaimsPrincipalExtensions.PersonId(User);
        var result = _tourService.CreateCampaign(tourDto);
        return CreateResponse(result);
    }
}