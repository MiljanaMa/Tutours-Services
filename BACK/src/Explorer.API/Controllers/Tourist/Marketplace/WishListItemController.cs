using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public;
using Explorer.Payments.Core.UseCases;
using Explorer.Stakeholders.Infrastructure.Authentication;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist.Marketplace;

[Authorize(Policy = "touristPolicy")]
[Route("api/tourist/itemswishlist")]
public class WishListItemController : BaseApiController
{
    private readonly IWishListItemService _wishListItemService;

    public WishListItemController(IWishListItemService wishListItemService)
    {
        _wishListItemService = wishListItemService;
    }

    [HttpGet]
    public ActionResult<PagedResult<WishListItemDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
    {
        var result = _wishListItemService.GetPaged(page, pageSize);
        return CreateResponse(result);
    }

    [HttpGet("byUser")]
    public ActionResult<PagedResult<WishListItemDto>> GetAllByUser()
    {
        var result = _wishListItemService.GetAllByUser(1, 1, User.PersonId());
        //var resultValue = Result.Ok(result);
        return CreateResponse(result);
    }


    [HttpPost]
    public ActionResult<WishListItemDto> Create([FromBody] WishListItemDto wishListItem)
    {
        wishListItem.UserId = User.PersonId();
        var result = _wishListItemService.Create(wishListItem);
        return CreateResponse(result);
    }

    [HttpPut("{id:int}")]
    public ActionResult<WishListItemDto> Update([FromBody] WishListItemDto wishListItem)
    {
        wishListItem.UserId = User.PersonId();
        var result = _wishListItemService.Update(wishListItem);
        return CreateResponse(result);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var result = _wishListItemService.Delete(id);
        return CreateResponse(result);
    }

    
}
