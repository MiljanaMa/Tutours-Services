using Explorer.Stakeholders.Infrastructure.Authentication;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.TourExecution;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist.TourExecution;

[Authorize(Policy = "touristPolicy")]
[Route("api/tourexecution/")]
public class TourLifecycleController : BaseApiController
{
    private readonly ITourLifecycleService _tourLifecycleService;

    public TourLifecycleController(ITourLifecycleService tourLifecycleService)
    {
        _tourLifecycleService = tourLifecycleService;
    }

    [HttpGet("activeTour")]
    public ActionResult<TourProgressDto> GetActiveByUser()
    {
        var result = _tourLifecycleService.GetActiveByUser(User.PersonId());
        return CreateResponse(result);
    }

    [HttpPost("start/{tourId:int}")]
    public ActionResult<TourProgressDto> StartTour([FromRoute] int tourId)
    {
        var result = _tourLifecycleService.StartTour(tourId, User.PersonId());
        return CreateResponse(result);
    }

    [HttpPut("abandonActive")]
    public ActionResult<TourProgressDto> AbandonActiveTour()
    {
        var result = _tourLifecycleService.AbandonActiveTour(User.PersonId());
        return CreateResponse(result);
    }

    [HttpPut("updateActive")]
    public ActionResult<TourProgressDto> UpdateActiveTour([FromBody] bool areRequiredEncountersDone)
    {
        var result = _tourLifecycleService.UpdateActiveTour(User.PersonId(), areRequiredEncountersDone);
        return CreateResponse(result);
    }
}