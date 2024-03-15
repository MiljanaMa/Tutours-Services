using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist.Marketplace;

[Route("api/marketplace/tour-discount")]
public class TourDiscountController : BaseApiController
{
    private readonly ITourDiscountService _discountService;

    public TourDiscountController(ITourDiscountService discountService)
    {
        _discountService = discountService;
    }

    [HttpPost]
    public ActionResult<TourDiscountDto> Create([FromBody] TourDiscountDto discountDto)
    {
        var result = _discountService.Create(discountDto);
        return !result.IsSuccess
            ? BadRequest(new
            {
                message = result.Reasons
            })
            : CreateResponse(result);
    }

    [HttpDelete("{tourId:int}")]
    public ActionResult Delete(int tourId)
    {
        var result = _discountService.Delete(tourId);
        return CreateResponse(result);
    }

    [HttpGet("tours/{discountId:int}")]
    public ActionResult<List<int>> GetToursFromOtherDiscounts(int discountId)
    {
        return CreateResponse(_discountService.GetToursFromOtherDiscounts(discountId));
    }
}