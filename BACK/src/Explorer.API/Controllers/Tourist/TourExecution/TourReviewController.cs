using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.TourExecution;
using Explorer.Tours.Core.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using System.Net.Http;
using System.Net;

namespace Explorer.API.Controllers.Tourist.TourExecution;

[Authorize(Policy = "touristPolicy")]
[Route("api/tourexecution/tourreview")]
public class TourReviewController : BaseApiController
{
    private readonly ITourReviewService _tourReviewService;
    private static HttpClient httpClient = new()
    {
        BaseAddress = new Uri(" http://localhost:8000/tourreview/"),
    };

    public TourReviewController(ITourReviewService tourReviewService)
    {
        _tourReviewService = tourReviewService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<TourReviewDto>>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
    {
        var httpResponse = await httpClient.GetAsync($"?page={page}&pageSize={pageSize}");

        if (httpResponse.IsSuccessStatusCode)
        {
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                var response = await httpResponse.Content.ReadFromJsonAsync<List<TourReviewDto>>();

                return Ok(new PagedResult<TourReviewDto>(response, response.Count));
            }

            return Ok();

        }
        else
        {
            return StatusCode((int)httpResponse.StatusCode, "Error while getting reviews");
        }
    }

    [HttpGet("{id:int}")]
    public ActionResult<TourReviewDto> Get(int id)
    {
        var result = _tourReviewService.Get(id);
        return CreateResponse(result);
    }

    [HttpPost]
    public async Task<ActionResult<TourReviewDto>> Create([FromBody] TourReviewDto tourReview)
    {
        tourReview.UserId = User.PersonId();
        var jsonContent = JsonSerializer.Serialize(tourReview);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var httpResponse = await httpClient.PostAsync("create", content);

        if (httpResponse.IsSuccessStatusCode)
        {
            var responseContent = await httpResponse.Content.ReadAsStringAsync();
            var createdReview = JsonSerializer.Deserialize<TourReviewDto>(responseContent);
            return Ok(createdReview);
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
    public ActionResult<TourReviewDto> Update([FromBody] TourReviewDto tourReview)
    {
        tourReview.UserId = User.PersonId();
        var result = _tourReviewService.Update(tourReview);
        return CreateResponse(result);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var result = _tourReviewService.Delete(id);
        return CreateResponse(result);
    }

    [HttpGet("tour/{tourId:long}")]
    [AllowAnonymous]
    public ActionResult<PagedResult<TourReviewDto>> GetByTourId(long tourId, [FromQuery] int page,
        [FromQuery] int pageSize)
    {
        var result = _tourReviewService.GetByTourId(tourId, page, pageSize);
        return CreateResponse(result);
    }

    [HttpPost("averageRate")]
    [AllowAnonymous]
    public ActionResult<double> CalculateAverageRate([FromBody] List<TourReviewDto> reviews)
    {
        var result = _tourReviewService.CalculateAverageRate(reviews);
        return CreateResponse(result);
    }
}