using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.TourAuthoring;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist.Marketplace;

[Authorize(Policy = "touristPolicy")]
[Route("api/tourist/publicKeypoint")]
public class PublicKeypointController : BaseApiController
{
    private readonly IPublicKeypointService _publicKeypointService;

    public PublicKeypointController(IPublicKeypointService publicKeypointService)
    {
        _publicKeypointService = publicKeypointService;
    }

    [HttpGet("filtered")]
    public ActionResult<PagedResult<ObjectDto>> GetPagedInRange([FromQuery] int page, [FromQuery] int pageSize,
        [FromQuery] FilterCriteriaDto filter)
    {
        var result = _publicKeypointService.GetPagedInRange(page, pageSize, filter);
        return CreateResponse(result);
    }
}