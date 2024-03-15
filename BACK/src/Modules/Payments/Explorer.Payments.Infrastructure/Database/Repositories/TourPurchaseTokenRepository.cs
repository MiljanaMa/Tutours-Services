using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Payments.Core.Domain;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Payments.Infrastructure.Database.Repositories;

public class TourPurchaseTokenRepository : CrudDatabaseRepository<TourPurchaseToken, PaymentsContext>,
    ITourPurchaseTokenRepository
{
    private readonly DbSet<TourPurchaseToken> _dbSet;

    public TourPurchaseTokenRepository(PaymentsContext dbContext) : base(dbContext)
    {
        _dbSet = dbContext.Set<TourPurchaseToken>();
    }

    public void AddRange(List<TourPurchaseToken> tokens)
    {
        _dbSet.AddRange(tokens);
        DbContext.SaveChanges();
    }

    public TourPurchaseToken GetByTourAndTourist(int tourId, int touristId)
    {
        return DbContext.TourPurchaseTokens.FirstOrDefault(t => t.TourId == tourId && t.TouristId == touristId);
    }

    public bool CheckIfPurchased(long tourId, long touristId)
    {
        return DbContext.TourPurchaseTokens.Any(t => t.TourId == tourId && t.TouristId == touristId);
    }
}