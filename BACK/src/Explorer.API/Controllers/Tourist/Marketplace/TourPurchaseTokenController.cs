using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public;
using Explorer.Payments.Core.Domain;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist.Marketplace;

[Authorize(Policy = "touristPolicy")]
[Route("api/marketplace/tours/token")]
public class TourPurchaseTokenController : BaseApiController
{
    private readonly ITourPurchaseTokenService _tourPurchaseTokenService;

    public TourPurchaseTokenController(ITourPurchaseTokenService tourPurchaseTokenService)
    {
        _tourPurchaseTokenService = tourPurchaseTokenService;
    }

    [HttpGet("{id:int}")]
    public ActionResult BuyShoppingCart(int id)
    {
        var result = _tourPurchaseTokenService.BuyShoppingCart(id, new List<CouponDto>());
        return CreateResponse(result);
    }

    [HttpGet]
    public ActionResult<PagedResult<TourPurchaseTokenDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
    {
        var result = _tourPurchaseTokenService.GetPaged(page, pageSize);
        return CreateResponse(result);
    }

    [HttpGet("check-purchase/{tourId:int}")]
    public ActionResult<bool> CheckIfPurchased([FromRoute] int tourId)
    {
        return _tourPurchaseTokenService.CheckIfPurchased(tourId, User.PersonId()).Value;

    }
}