using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.API.Dtos
{
    public class CouponDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public double Discount { get; set; }
        public int? TourId { get; set; }
        public int TouristId { get; set; }
        public int AuthorId { get; set; }
        public DateOnly ExpiryDate { get; set; }

        public CouponDto() { }

        public CouponDto(int id, string code, double discount, int tourId, int touristId, int authorId , DateOnly expiryDate)
        {
            Id = id;
            Code = code;
            Discount = discount;
            TourId = tourId;
            TouristId = touristId;
            AuthorId = authorId;
            ExpiryDate = expiryDate;
        }
    }
}
