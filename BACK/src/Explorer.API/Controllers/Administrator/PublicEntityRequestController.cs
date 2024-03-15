using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.TourAuthoring;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Administrator;

[Authorize(Policy = "administratorPolicy")]
[Route("api/administrator/publicEntityRequests")]
public class PublicEntityRequestController : BaseApiController
{
    private readonly IPublicEntityRequestService _publicEntityRequestService;

    public PublicEntityRequestController(IPublicEntityRequestService publicEntityRequestService)
    {
        _publicEntityRequestService = publicEntityRequestService;
    }

    [HttpGet]
    public ActionResult<PagedResult<PublicEntityRequestDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
    {
        var result = _publicEntityRequestService.GetPaged(page, pageSize);
        return CreateResponse(result);
    }

    [HttpGet("{id:int}")]
    public ActionResult<PublicEntityRequestDto> Get(int id)
    {
        var result = _publicEntityRequestService.Get(id);
        return CreateResponse(result);
    }

    [HttpPut("approve")]
    public ActionResult<PublicEntityRequestDto> Approve(PublicEntityRequestDto publicEntityRequestDto)
    {
        var result = _publicEntityRequestService.Approve(publicEntityRequestDto);
        return CreateResponse(result);
    }

    [HttpPut("decline")]
    public ActionResult<PublicEntityRequestDto> Decline([FromBody] PublicEntityRequestDto publicEntityRequestDto)
    {
        var result = _publicEntityRequestService.Decline(publicEntityRequestDto);
        return CreateResponse(result);
    }
}