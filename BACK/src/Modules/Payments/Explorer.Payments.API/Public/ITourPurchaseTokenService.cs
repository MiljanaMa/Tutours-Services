using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payments.API.Dtos;
using FluentResults;

namespace Explorer.Payments.API.Public;

public interface ITourPurchaseTokenService
{
    Result BuyShoppingCart(int shoppingCartId, List<CouponDto> selectedCoupons);
    Result<PagedResult<TourPurchaseTokenDto>> GetPaged(int page, int pageSize);
    Result<bool> CheckIfPurchased(long userId, long tourId);
}