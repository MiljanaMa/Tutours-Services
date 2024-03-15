using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payments.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Core.Domain.RepositoryInterfaces
{
    public interface ICouponRepository : ICrudRepository<Coupon>
    {
        Result<PagedResult<Coupon>> GetCouponForTourAndTourist(int page, int pageSize, int tourId, int touristId);
        Result<PagedResult<Coupon>> GetCouponsForAuthor(int page, int pageSize, int authorId);
        Result<PagedResult<Coupon>> GetCouponForAuthorAndTourist(int page, int pageSize, int authorId, int touristId);
        Coupon GetCouponByTourId(int tourId);
    }
}
