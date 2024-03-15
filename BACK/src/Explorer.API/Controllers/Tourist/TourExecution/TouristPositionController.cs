using Explorer.Stakeholders.Infrastructure.Authentication;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.TourExecution;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist.TourExecution;

[Authorize(Policy = "touristPolicy")]
[Route("api/tourist/position")]
public class TouristPositionController : BaseApiController
{
    private readonly ITouristPositionService _touristPositionService;

    public TouristPositionController(ITouristPositionService touristPositionService)
    {
        _touristPositionService = touristPositionService;
    }

    [HttpGet]
    public ActionResult<TouristPositionDto> GetByUser()
    {
        var result = _touristPositionService.GetByUser(User.PersonId());
        return CreateResponse(result);
    }

    [HttpPost]
    public ActionResult<TourPreferenceDto> Create([FromBody] TouristPositionDto touristPosition)
    {
        touristPosition.UserId = User.PersonId();
        var result = _touristPositionService.Create(touristPosition);
        return CreateResponse(result);
    }

    [HttpPut]
    public ActionResult<TourPreferenceDto> Update([FromBody] TouristPositionDto touristPosition)
    {
        touristPosition.UserId = User.PersonId();
        var result = _touristPositionService.Update(touristPosition);
        return CreateResponse(result);
    }
}