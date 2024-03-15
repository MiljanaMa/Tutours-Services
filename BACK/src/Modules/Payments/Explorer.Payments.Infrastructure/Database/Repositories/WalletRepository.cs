using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Payments.Core.Domain;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Infrastructure.Database.Repositories
{
    public class WalletRepository: CrudDatabaseRepository<Wallet, PaymentsContext>, IWalletRepository
    {
        private readonly DbSet<Wallet> _dbSet;

        public WalletRepository(PaymentsContext dbContext) : base(dbContext)
        {
            _dbSet = dbContext.Set<Wallet>();
        }

        public Wallet GetByUser(int userId)
        {
            var wallet = _dbSet.AsNoTracking().FirstOrDefault(tp => tp.UserId == userId);
            return wallet;
        }
    }
}
