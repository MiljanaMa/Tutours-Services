using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Payments.Core.Domain;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Payments.Infrastructure.Database.Repositories;

public class OrderItemRepository : CrudDatabaseRepository<OrderItem, PaymentsContext>, IOrderItemRepository
{
    protected readonly PaymentsContext _dbContext;
    private readonly DbSet<OrderItem> _dbSet;

    public OrderItemRepository(PaymentsContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
        _dbSet = dbContext.Set<OrderItem>();
    }

    public PagedResult<OrderItem> GetByUser(int page, int pageSize, int userId)
    {
        var task = _dbSet.Where(k => k.UserId == userId).GetPagedById(page, pageSize);
        task.Wait();
        return task.Result;
    }

    public void RemoveRange(List<int> orderIds)
    {
        _dbSet.RemoveRange(_dbSet.Where(r => orderIds.Contains(Convert.ToInt32(r.Id))));
    }
}