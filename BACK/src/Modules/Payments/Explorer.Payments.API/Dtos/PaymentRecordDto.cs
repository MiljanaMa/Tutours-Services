using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.API.Dtos;

public class PaymentRecordDto
{
    public int Id { get; set; }
    public int TourId { get; set; }
    public int UserId { get; set; }
    public double TourPrice { get; set; }
    public DateTimeOffset PaymentTime { get; set; }
}
