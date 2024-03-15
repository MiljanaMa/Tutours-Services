using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payments.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.API.Public
{
    public interface IBundlePriceService
    {
      //  Result<PagedResult<BundlePriceDto>> GetPaged(int page, int pageSize);
       // Result<BundlePriceDto> Create(BundlePriceDto bundle);
       // Result<BundlePriceDto> Update(BundlePriceDto bundle);

       // Result<BundlePriceDto> Get(long id);
      //  Result Delete(int id);

        Result<BundlePriceDto> Create(BundlePriceDto price);
        Result<BundlePriceDto> GetPriceForBundle(long bundleId);
    }
}
