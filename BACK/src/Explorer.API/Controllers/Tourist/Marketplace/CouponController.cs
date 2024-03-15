using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public;
using Explorer.Payments.Core.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist.Marketplace
{
    [Route("api/marketplace/coupons")]
    public class CouponController : BaseApiController
    {
        private readonly ICouponService _couponService;

        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        [HttpPost]
        [Authorize(Roles = "author")]
        public ActionResult<CouponDto> Create([FromBody] CouponDto coupon)
        {
            var result = _couponService.Create(coupon);
            return CreateResponse(result);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "author, tourist")]
        public ActionResult Delete(int id)
        {
            var result = _couponService.Delete(id);
            return CreateResponse(result);
        }

        [HttpPut]
        [Authorize(Roles = "author")]
        public ActionResult<CouponDto> Update([FromBody] CouponDto coupon)
        {
            var result = _couponService.Update(coupon);
            return CreateResponse(result);
        }

        [HttpGet("getForTourAndTourist/{tourId:int}/{touristId:int}")]
        [Authorize(Roles = "tourist")]
        public ActionResult<PagedResult<CouponDto>> GetCouponsForTourAndTourist(int page,  int pageSize, int tourId, int touristId)
        {
            var result = _couponService.GetCouponForTourAndTourist(page, pageSize, tourId, touristId);
            return CreateResponse(result);
        }

        [HttpGet("getForAuthor/{authorId:int}")]
        [Authorize(Roles = "author")]
        public ActionResult<PagedResult<CouponDto>> GetCouponsForAuthor(int page, int pageSize, int authorId)
        {
            var result = _couponService.GetCouponsForAuthor(page, pageSize, authorId);
            return CreateResponse(result);
        }
    }
}
