using Explorer.Payments.API.Dtos;
using FluentResults;

namespace Explorer.Payments.API.Public;

public interface ITourDiscountService
{
    Result<TourDiscountDto> Create(TourDiscountDto tourDiscountDto);
    Result Delete(int tourId);

    public Result<List<int>> GetToursFromOtherDiscounts(int discountId);
}