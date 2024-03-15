using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Public.MarketPlace
{
    public interface IBundleService
    {
        Result<PagedResult<BundleDto>> GetPaged(int page, int pageSize);
        Result<BundleDto> Create(BundleDto bundle);
        Result<BundleDto> Update(BundleDto bundle);

        Result<BundleDto> Get(long id);
        Result Delete(int id);

        Result<BundleDto> AddTourToBundle(long tourId, long bundleId);
        Result  RemoveTourFromBundle(long tourId, long bundleId);

        Result<BundleDto> PublishBundle(long bundleId);
        Result<BundleDto> ArchiveBundle(long bundleId);

        Result<double> CalculatePrice(long bundleId);


    }
}
