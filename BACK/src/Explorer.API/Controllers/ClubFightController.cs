using Explorer.Encounters.API.Public;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.UseCases;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers;

[Authorize(Policy = "userPolicy")]
[Route("api/tourist/fight")]
public class ClubFightController : BaseApiController
{
    private readonly IClubFightService _clubFightService;
    private readonly IXPService _xpService;

    public ClubFightController(IClubFightService clubFightService, IXPService xpService)
    {
        _clubFightService = clubFightService;
        _xpService = xpService;
    }

    [HttpGet("{fightId:int}")]
    public ActionResult<ClubFightDto> GetById([FromRoute] int fightId)
    {
        var result = _clubFightService.GetWithClubs(fightId);
        return CreateResponse(result);
    }

    [HttpGet("all/{clubId:int}")]
    public ActionResult<List<ClubFightDto>> GetAllByClub([FromRoute] int clubId)
    {
        var result = _clubFightService.GetAllByClub(clubId);
        return CreateResponse(result);
    }
    
    [HttpGet("end/{fightId:int}")]
    public void EndFight([FromRoute] int fightId)
    {
        // var fightResult = _clubFightService.Get(fightId);
        // ClubFightDto fightDto = fightResult.Value;
        // fightDto.EndOfFight = DateTime.Now;
        // _clubFightService.Update(fightDto);
        _xpService.UpdateFights(true);
    }
}