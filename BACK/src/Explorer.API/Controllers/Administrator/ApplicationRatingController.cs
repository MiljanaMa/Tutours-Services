using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Administrator;

[Authorize(Policy = "administratorPolicy")]
[Route("api/administrator/appRating")]
public class ApplicationRatingController : BaseApiController
{
    private readonly IApplicationRatingService _ratingService;

    public ApplicationRatingController(IApplicationRatingService ratingService)
    {
        _ratingService = ratingService;
    }

    [HttpGet]
    public ActionResult<PagedResult<ApplicationRatingDto>> GetAll([FromQuery] int page, [FromQuery] int pagesize)
    {
        var result = _ratingService.GetPaged(page, pagesize);
        return CreateResponse(result);
    }
}