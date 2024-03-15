using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Payments.Core.Domain;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Infrastructure.Database.Repositories;

public class WishListItemRepository : CrudDatabaseRepository<WishListItem, PaymentsContext>, IWishListItemRepository
{
    protected readonly PaymentsContext _dbContext;
    protected readonly DbSet<WishListItem> _dbSet;

    public WishListItemRepository(PaymentsContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
        _dbSet = dbContext.Set<WishListItem>();
    }

    public PagedResult<WishListItem> GetByUser(int page, int pageSize, int userId)
    {
        var result = _dbSet.Where(r => r.UserId == userId).GetPagedById(page, pageSize);
        result.Wait();
        return result.Result;
    }

    public void RemoveRange(List<int> wishListItemsIds)
    {
        _dbSet.RemoveRange(_dbSet.Where(r => wishListItemsIds.Contains(Convert.ToInt32(r.UserId))));
    }

    
}
