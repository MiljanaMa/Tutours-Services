using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Dtos.Enums;
using Explorer.Tours.API.Public.TourAuthoring;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Author;

[Authorize(Policy = "authorPolicy")]
[Route("api/author/publicEntityRequests")]
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

    [HttpPost("createKeypointRequest")]
    public ActionResult<PublicEntityRequestDto> CreateKeypointRequest(
        [FromBody] PublicEntityRequestDto publicEntityRequestDto)
    {
        publicEntityRequestDto.UserId = User.PersonId();
        var result = _publicEntityRequestService.CreateKeypointRequest(publicEntityRequestDto);
        return CreateResponse(result);
    }

    [HttpPost("createObjectRequest")]
    public ActionResult<PublicEntityRequestDto> CreateObjectRequest(
        [FromBody] PublicEntityRequestDto publicEntityRequestDto)
    {
        publicEntityRequestDto.UserId = User.PersonId();
        var result = _publicEntityRequestService.CreateObjectRequest(publicEntityRequestDto);
        return CreateResponse(result);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id) // za cancel
    {
        var result = _publicEntityRequestService.Delete(id);
        return CreateResponse(result);
    }

    [HttpGet("entity/{entityId}/{entityType}")]
    public ActionResult<PublicEntityRequestDto> GetByEntityId([FromRoute] int entityId, [FromRoute] int entityType)
    {
        var result = _publicEntityRequestService.GetByEntityId(entityId, (EntityType)entityType);
        return CreateResponse(result);
    }
}