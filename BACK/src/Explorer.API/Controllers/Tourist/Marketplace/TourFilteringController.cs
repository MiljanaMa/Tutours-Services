using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.MarketPlace;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist.Marketplace;

[Authorize(Policy = "touristPolicy")]
[Route("api/marketplace/tours/filter")]
public class TourFilteringController : BaseApiController
{
    private readonly ITourFilteringService _filteringService;

    public TourFilteringController(ITourFilteringService filteringService)
    {
        _filteringService = filteringService;
    }

    [HttpGet]
    public ActionResult<PagedResult<TourDto>> GetFilteredTours([FromQuery] int page, [FromQuery] int pageSize,
        [FromQuery] FilterCriteriaDto filter)
    {
        var result = _filteringService.GetFilteredTours(page, pageSize, filter);
        return CreateResponse(result);
    }
}