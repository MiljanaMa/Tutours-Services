using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public.Tourist;
using Explorer.Stakeholders.Infrastructure.Authentication;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist;

[Authorize(Policy = "userPolicy")]
[Route("api/tourist/clubs")]
public class ClubController : BaseApiController
{
    private readonly IClubService _clubService;


    public ClubController(IClubService clubService)
    {
        _clubService = clubService;
    }


    [HttpGet]
    public ActionResult<PagedResult<ClubDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
    {
        var result = _clubService.GetAll(page, pageSize);
        return CreateResponse(result);
    }


    [HttpGet("byUser")]
    public ActionResult<PagedResult<ClubDto>> GetAllByUser([FromQuery] int page, [FromQuery] int pageSize)
    {
        var result = _clubService.GetAllByUser(page, pageSize, User.PersonId());
        var resultValue = Result.Ok(result);
        return CreateResponse(resultValue);
    }


    [HttpPost]
    public ActionResult<ClubDto> Create([FromBody] ClubDto club)
    {
        var result = _clubService.Create(club);
        return CreateResponse(result);
    }

    [HttpPut("{id:int}")]
    public ActionResult<ClubDto> Update([FromBody] ClubDto club)
    {
        var result = _clubService.Update(club);
        return CreateResponse(result);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var result = _clubService.Delete(id);
        return CreateResponse(result);
    }
    
    [HttpGet("{clubId:int}")]
    public ActionResult<ClubDto> GetById([FromRoute] int clubId)
    {
        var result = _clubService.GetWithMembers(clubId);
        return CreateResponse(result);
    }
}