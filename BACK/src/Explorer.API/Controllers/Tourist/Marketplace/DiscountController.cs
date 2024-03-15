using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Infrastructure.Authentication;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Explorer.API.Controllers.Tourist.Marketplace;

[Route("api/marketplace/discounts")]
public class DiscountController : BaseApiController
{
    private readonly IDiscountService _discountService;
    public DiscountController(IDiscountService discountService) : base()
    {
        _discountService = discountService;
    }
    [HttpGet]
    public ActionResult<PagedResult<DiscountDto>> GetAll(int page, int pageSize)
    {
        var result = _discountService.GetAllWithTours(page, pageSize);
        return CreateResponse(result);
    }

    [HttpPost]
    public ActionResult<DiscountDto> Create([FromBody] DiscountDto discountDto)
    {
        discountDto.UserId = ClaimsPrincipalExtensions.PersonId(User);
        var result = _discountService.Create(discountDto);
        return CreateResponse(result);
    }

    [HttpPut]
    public ActionResult<DiscountDto> Update([FromBody] DiscountDto discountDto)
    {
        var result = _discountService.Update(discountDto);
        return CreateResponse(result);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var result = _discountService.Delete(id);
        return CreateResponse(result);
    }

    [HttpGet("author-discounts")]
    public ActionResult<PagedResult<DiscountDto>> GetDiscountsByAuthor([FromQuery] int page, [FromQuery] int pageSize)
    {
        var authorId = ClaimsPrincipalExtensions.PersonId(User);
        var result = _discountService.GetDiscountsByAuthor(authorId, page, pageSize);
        return CreateResponse(result);
    }
}