using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public.Tourist;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist;

[Authorize(Policy = "touristPolicy")]
[Route("api/tourist/clubJoinRequest")]
public class ClubJoinRequestController : BaseApiController
{
    private readonly IClubJoinRequestService _requestService;

    public ClubJoinRequestController(IClubJoinRequestService requestService)
    {
        _requestService = requestService;
    }

    [HttpGet("all")]
    public ActionResult<PagedResult<ClubJoinRequestDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
    {
        var result = _requestService.GetPaged(page, pageSize);
        return CreateResponse(result);
    }

    [HttpGet]
    public ActionResult<ClubJoinRequestDto> GetAllByUser()
    {
        var result = _requestService.GetAllByUser(User.PersonId());
        return CreateResponse(result);
    }

    [HttpGet("{clubId}")]
    public ActionResult<PagedResult<ClubJoinRequestDto>> GetAllByClub([FromRoute] int clubId)
    {
        var result = _requestService.GetAllByClub(clubId);
        return CreateResponse(result);
    }

    [HttpPost]
    public ActionResult<ClubJoinRequestDto> Create([FromBody] ClubDto club)
    {
        var result = _requestService.Create(club, User.PersonId());
        return CreateResponse(result);
    }

    [HttpPut]
    public ActionResult<ClubJoinRequestDto> Update([FromBody] ClubJoinRequestDto request)
    {
        var result = _requestService.Update(request);
        return CreateResponse(result);
    }
}