using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.API.Dtos
{
    public class BundlePriceDto
    {
        public long Id { get; set; }
        public long BundleId { get; set; }
        
        public double TotalPrice { get; set; }
        
    }
}
