using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payments.API.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Core.Domain.RepositoryInterfaces
{
    public interface IBundlePriceRepository : ICrudRepository<BundlePrice>
    {
        BundlePrice GetPriceById(long bundleId);
    }
}
