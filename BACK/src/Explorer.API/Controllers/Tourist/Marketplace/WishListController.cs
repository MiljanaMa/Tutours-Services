using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public;
using Explorer.Stakeholders.Infrastructure.Authentication;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist.Marketplace;


[Authorize(Policy = "touristPolicy")]
[Route("api/tourist/wishlist")]
public class WishListController : BaseApiController
{
    private readonly IWishListService _wishListService;

    public WishListController(IWishListService wishListService)
    {
        _wishListService = wishListService;
    }

    [HttpGet("byUser")]
    public ActionResult<PagedResult<WishListDto>> GetByUser()
    {
        var result = _wishListService.GetByUser(User.PersonId());
        var resultValue = Result.Ok(result);
        return CreateResponse(resultValue);
    }

    [HttpPost]
    public ActionResult<WishListDto> Create([FromBody] WishListDto wishList)
    {
        wishList.UserId = User.PersonId();
        var result = _wishListService.Create(wishList);
        return CreateResponse(result);
    }

    [HttpPut("{id:int}")]
    public ActionResult<ShoppingCartDto> Update([FromBody] WishListDto wishList)
    {
        wishList.UserId = User.PersonId();
        var result = _wishListService.Update(wishList);
        return CreateResponse(result);
    }
}
