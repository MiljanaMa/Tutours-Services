using Explorer.BuildingBlocks.Core.Domain;
using Explorer.Payments.Core.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Core.Domain
{
    public class BundlePrice : Entity
    {
        public long BundleId { get; init; }
        public double TotalPrice { get; init; }
        
        public BundlePrice() { }

        public BundlePrice(long id ,long bundleId,double totalPrice)
        {
            Id = id;
            BundleId = bundleId;
            TotalPrice = totalPrice;
           
            Validate();
        }

        private void Validate()
        {
           
            if (TotalPrice < 0) throw new Exception("Invalid total price");
           

        }
    }
}
