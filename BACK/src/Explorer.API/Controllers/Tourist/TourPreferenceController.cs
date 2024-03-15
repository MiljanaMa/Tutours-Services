using Explorer.Stakeholders.Infrastructure.Authentication;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.MarketPlace;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist;

[Authorize(Policy = "touristPolicy")]
[Route("api/tourist/tourPreference")]
public class TourPreferenceController : BaseApiController
{
    private readonly ITourPreferenceService _tourPreferenceService;

    public TourPreferenceController(ITourPreferenceService tourPreferenceService)
    {
        _tourPreferenceService = tourPreferenceService;
    }

    [HttpGet]
    public ActionResult<TourPreferenceDto> GetByUser()
    {
        var result = _tourPreferenceService.GetByUser(User.PersonId());
        return CreateResponse(result);
    }

    [HttpPost]
    public ActionResult<TourPreferenceDto> Create([FromBody] TourPreferenceDto tourPreference)
    {
        tourPreference.UserId = User.PersonId();
        var result = _tourPreferenceService.Create(tourPreference);
        return CreateResponse(result);
    }

    [HttpPut]
    public ActionResult<TourPreferenceDto> Update([FromBody] TourPreferenceDto tourPreference)
    {
        tourPreference.UserId = User.PersonId();
        var result = _tourPreferenceService.Update(tourPreference);
        return CreateResponse(result);
    }

    [HttpDelete]
    public ActionResult Delete()
    {
        var userId = User.PersonId();
        var result = _tourPreferenceService.Delete(userId);
        return CreateResponse(result);
    }
}