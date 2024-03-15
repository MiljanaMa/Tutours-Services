using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payments.API.Dtos;
using FluentResults;

namespace Explorer.Payments.API.Public;

public interface IDiscountService
{
    Result<DiscountDto> Create(DiscountDto discount);
    Result<DiscountDto> Update(DiscountDto discount);
    Result Delete(int id);
    Result<double> GetDiscountForTour(int tourId);
    Result<PagedResult<DiscountDto>> GetDiscountsByAuthor(int userId, int page, int pageSize);
    Result<PagedResult<DiscountDto>> GetAllWithTours(int page, int pageSize);
}