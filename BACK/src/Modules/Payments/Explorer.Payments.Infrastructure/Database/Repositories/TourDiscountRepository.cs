using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Payments.Core.Domain;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Payments.Infrastructure.Database.Repositories;

public class TourDiscountRepository : CrudDatabaseRepository<TourDiscount, PaymentsContext>, ITourDiscountRepository
{
    private readonly PaymentsContext _dbContext;
    private readonly DbSet<TourDiscount> _dbSet;

    public TourDiscountRepository(PaymentsContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
        _dbSet = dbContext.Set<TourDiscount>();
    }

    public new TourDiscount Create(TourDiscount TourDiscount)
    {
        var existingTourDiscount = _dbContext.TourDiscounts.FirstOrDefault(ts => ts.TourId == TourDiscount.TourId);

        if (existingTourDiscount == null)
        {
            _dbContext.TourDiscounts.Add(TourDiscount);
            _dbContext.SaveChanges();
            return TourDiscount;
        }

        return null;
    }

    public void Delete(int tourId)
    {
        var TourDiscountToRemove = _dbSet.SingleOrDefault(ts => ts.TourId == tourId);

        if (TourDiscountToRemove == null) return;
        _dbSet.Remove(TourDiscountToRemove);
        _dbContext.SaveChanges();
    }

    public List<int> GetToursFromOtherDiscounts(int discountId)
    {
        return _dbSet.Where(t => t.DiscountId != discountId).Select(td => td.TourId).ToList();
    }
}