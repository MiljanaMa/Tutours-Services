using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public.Tourist;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist;

[Authorize(Policy = "touristPolicy")]
[Route("api/tourist/clubInvitation")]
public class ClubInvitationController : BaseApiController
{
    private readonly IClubInvitationService _clubInvitationService;

    public ClubInvitationController(IClubInvitationService clubInvitationService)
    {
        _clubInvitationService = clubInvitationService;
    }

    [HttpGet]
    public ActionResult<PagedResult<ClubInvitationDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
    {
        var result = _clubInvitationService.GetPaged(page, pageSize);
        return CreateResponse(result);
    }

    [HttpPost]
    public ActionResult<ClubInvitationDto> Create([FromBody] ClubInvitationDto invitation)
    {
        invitation.Status = InvitationStatus.PENDING;
        var result = _clubInvitationService.Create(invitation);
        return CreateResponse(result);
    }

    [HttpPut("{id:int}")]
    public ActionResult<ClubInvitationDto> Update([FromBody] ClubInvitationDto invitation)
    {
        var result = _clubInvitationService.Update(invitation);
        return CreateResponse(result);
    }
}