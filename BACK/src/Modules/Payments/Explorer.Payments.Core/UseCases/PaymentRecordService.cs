using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public;
using Explorer.Payments.Core.Domain;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Core.UseCases;

public class PaymentRecordService : CrudService<PaymentRecordDto, PaymentRecord>, IPaymentRecordService
{
    protected readonly IPaymentRecordRepository _paymentRecordRepository;

    public PaymentRecordService(IPaymentRecordRepository paymentRecordRepository, IMapper mapper) : base(paymentRecordRepository, mapper)
    {
        _paymentRecordRepository = paymentRecordRepository; 
    }


}
