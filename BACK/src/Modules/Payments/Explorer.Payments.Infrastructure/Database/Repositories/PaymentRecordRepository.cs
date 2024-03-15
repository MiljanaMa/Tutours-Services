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

public class PaymentRecordRepository : CrudDatabaseRepository<PaymentRecord, PaymentsContext>, IPaymentRecordRepository
{
    private readonly DbSet<PaymentRecord> _dbSet;

    public PaymentRecordRepository(PaymentsContext dbContext) : base(dbContext)
    {
        _dbSet = dbContext.Set<PaymentRecord>();
    }

    public void AddRange(List<PaymentRecord> paymentRecords)
    {
        _dbSet.AddRange(paymentRecords);
        DbContext.SaveChanges();
    }
}
