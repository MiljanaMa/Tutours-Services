using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.TourAuthoring;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist.Marketplace;

[Authorize(Policy = "touristPolicy")]
[Route("api/tourist/object")]
public class PublicObjectController : BaseApiController
{
    private readonly IObjectService _objectService;

    public PublicObjectController(IObjectService objectService)
    {
        _objectService = objectService;
    }

    [HttpGet("filtered")]
    public ActionResult<PagedResult<ObjectDto>> GetPagedInRange([FromQuery] int page, [FromQuery] int pageSize,
        [FromQuery] FilterCriteriaDto filter)
    {
        var result = _objectService.GetPublicPagedInRange(page, pageSize, filter);
        return CreateResponse(result);
    }
}