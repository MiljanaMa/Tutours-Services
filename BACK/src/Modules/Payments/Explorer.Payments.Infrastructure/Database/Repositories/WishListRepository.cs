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

public class WishListRepository : CrudDatabaseRepository<WishList, PaymentsContext>, IWishListRepository
{
    protected readonly PaymentsContext _dbContext;
    protected readonly DbSet<WishList> _dbSet;
    public WishListRepository(PaymentsContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
        _dbSet = dbContext.Set<WishList>();
    }

    public WishList GetByUser(int userId)
    {
        var result = _dbSet.AsNoTracking().FirstOrDefault(x => x.UserId == userId);
        return result;
    }


}
