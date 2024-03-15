using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payments.API.Dtos;

namespace Explorer.Payments.Core.Domain.RepositoryInterfaces;

public interface ITourDiscountRepository : ICrudRepository<TourDiscount>
{
    public void Delete(int tourId);
    public List<int> GetToursFromOtherDiscounts(int discountId);

}