using Explorer.BuildingBlocks.Core.UseCases;

namespace Explorer.Payments.Core.Domain.RepositoryInterfaces;

public interface IOrderItemRepository : ICrudRepository<OrderItem>
{
    public PagedResult<OrderItem> GetByUser(int page, int pageSize, int userId);
    public void RemoveRange(List<int> orderIds);
}