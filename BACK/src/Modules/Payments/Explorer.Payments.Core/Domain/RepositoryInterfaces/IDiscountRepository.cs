using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public;

namespace Explorer.Payments.Core.Domain.RepositoryInterfaces;

public interface IDiscountRepository : ICrudRepository<Discount>
{
    public double GetDiscountForTour(int tourId);
    public PagedResult<Discount> GetDiscountsByAuthor(int userId, int page, int pageSize);
    public PagedResult<Discount> GetAllWithTours(int page, int pageSize);
}