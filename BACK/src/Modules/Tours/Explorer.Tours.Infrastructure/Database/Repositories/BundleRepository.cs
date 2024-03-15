using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Infrastructure.Database.Repositories
{
    public class BundleRepository : CrudDatabaseRepository<Bundle, ToursContext>, IBundleRepository
    {
        private readonly DbSet<Bundle> _dbSet;

        public BundleRepository(ToursContext dbContext) : base(dbContext)
        {
            _dbSet = dbContext.Set<Bundle>();
        }

        public Bundle getFullBundle(long bundleId)
        {
            var bundle = _dbSet.Where(b => b.Id == bundleId).Include(t => t.Tours).FirstOrDefault();
        
        return bundle;
        }

        public PagedResult<ICollection<Tour>> GetToursForBundle(long bundleId, int page, int pageSize)
        {
            var pagedResult = _dbSet
                .Where(b => b.Id == bundleId)
                .Include(b => b.Tours)
                .Select(b => b.Tours)
                .GetPaged(page, pageSize);
            
            pagedResult.Wait();

            return pagedResult.Result;
        }

    }
}
