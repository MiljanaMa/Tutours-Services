using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Core.Domain;

public class PaymentRecord : Entity
{

    public PaymentRecord()
    {
    }

    public PaymentRecord(int tourId, int userId, double tourPrice, DateTimeOffset paymentTime)
    {
        TourId = tourId;
        UserId = userId;
        TourPrice = tourPrice;
        PaymentTime = paymentTime;
    }

    public int TourId { get; set; }
    public int UserId { get; set; }
    public double TourPrice { get; set; }
    public DateTimeOffset PaymentTime { get; set; }

}
