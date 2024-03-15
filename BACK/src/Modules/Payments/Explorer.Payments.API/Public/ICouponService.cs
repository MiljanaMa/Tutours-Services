using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payments.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.API.Public
{
    public interface ICouponService
    {
        Result<CouponDto> Create(CouponDto coupon);
        Result<CouponDto> Update(CouponDto coupon);
        Result Delete(int id);
        Result<PagedResult<CouponDto>> GetCouponForTourAndTourist(int page, int pageSize, int tourId, int touristId);
        Result<PagedResult<CouponDto>> GetCouponsForAuthor(int page, int pageSize, int authorId);
        CouponDto GetCouponByTourId(int tourId);

    }
}
