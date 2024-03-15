using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public;
using Explorer.Payments.Core.Domain;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Payments.Infrastructure.Database.Repositories;

public class DiscountRepository: CrudDatabaseRepository<Discount, PaymentsContext>, IDiscountRepository
{
    protected readonly PaymentsContext _dbContext;
    private readonly DbSet<Discount> _dbSet;

    public DiscountRepository(PaymentsContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
        _dbSet = dbContext.Set<Discount>();
    }

    public double GetDiscountForTour(int tourId)
    {
        var DiscountId = _dbContext.TourDiscounts
            .Where(ts => ts.TourId == tourId)
            .Select(ts => ts.DiscountId)
            .FirstOrDefault();

        if (DiscountId == 0) return 0;
        var Discount = _dbContext.Discounts.Find(DiscountId);
        if (Discount != null)
            return Discount.Percentage;

        return 0;
    }

    public PagedResult<Discount> GetDiscountsByAuthor(int userId, int page, int pageSize)
    {
        var task = _dbSet.Where(d => d.UserId == userId)
            .Include(d => d.TourDiscounts)
            .GetPaged(page, pageSize);
        task.Wait();
        return task.Result;
    }

    public PagedResult<Discount> GetAllWithTours(int page, int pageSize)
    {
        var task = _dbSet.Include(s => s.TourDiscounts).GetPagedById(page, pageSize);
        task.Wait();
        return task.Result;
    }

}