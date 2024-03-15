using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.TourAuthoring;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist.TourExecution;

[Authorize(Policy = "touristPolicy")]
[Route("api/tourist/tours/")]
public class TourController : BaseApiController
{
    private readonly ITourService _tourService;

    public TourController(ITourService tourService)
    {
        _tourService = tourService;
    }

    [HttpGet("{tourId:int}")]
    public ActionResult<TourDto> GetById([FromRoute] int tourId)
    {
        var result = _tourService.Get(tourId);
        return CreateResponse(result);
    }
}