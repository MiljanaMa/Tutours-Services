using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers;

[Authorize(Policy = "userPolicy")]
[Route("api/club-challenge-request")]
public class ClubChallengeRequestController : BaseApiController
{
    private readonly IClubChallengeRequestService _requestService;

    public ClubChallengeRequestController(IClubChallengeRequestService requestService)
    {
        _requestService = requestService;
    }

    [HttpPost]
    public ActionResult<ClubChallengeRequestDto> Create([FromBody] ClubChallengeRequestDto challengeRequestDto)
    {
        var result = _requestService.Create(challengeRequestDto);
        return CreateResponse(result);
    }

    [HttpPut("accept")]
    public ActionResult<ClubChallengeRequestDto> AcceptChallenge([FromBody] ClubChallengeRequestDto challengeRequestDto)
    {
        var result = _requestService.AcceptChallenge(challengeRequestDto);
        return CreateResponse(result);
    }
    
    [HttpGet("club/{clubId:long}")]
    public ActionResult<List<ClubChallengeRequestDto>> GetChallengesForClub([FromRoute] long clubId)
    {
        var result = _requestService.GetCurrentChallengesForClub(clubId);
        return CreateResponse(result);
    }
}