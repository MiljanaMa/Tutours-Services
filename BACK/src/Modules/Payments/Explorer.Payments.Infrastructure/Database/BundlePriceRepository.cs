using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Payments.Core.Domain;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Infrastructure.Database
{
    public class BundlePriceRepository : CrudDatabaseRepository<BundlePrice, PaymentsContext>, IBundlePriceRepository
    {
        private readonly DbSet<BundlePrice> _dbSet;

        public BundlePriceRepository(PaymentsContext dbContext) : base(dbContext)
        {
            _dbSet = dbContext.Set<BundlePrice>();
        }
        public BundlePrice GetPriceById(long bundleId)
        {
            return _dbSet.Where(b => b.BundleId == bundleId).FirstOrDefault();
        }
    }
}
