using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Payments.Core.Domain;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Payments.Infrastructure.Database.Repositories;

public class ShoppingCartRepository : CrudDatabaseRepository<ShoppingCart, PaymentsContext>, IShoppingCartRepository
{
    private readonly DbSet<ShoppingCart> _dbSet;

    public ShoppingCartRepository(PaymentsContext dbContext) : base(dbContext)
    {
        _dbSet = dbContext.Set<ShoppingCart>();
    }

    public ShoppingCart GetByUser(int userId)
    {
        var shoppingCart = _dbSet.AsNoTracking().FirstOrDefault(tp => tp.UserId == userId);
        return shoppingCart;
    }
}