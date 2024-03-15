using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public;
using Explorer.Payments.Core.Domain;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Core.UseCases
{
    public class BundlePriceService : CrudService<BundlePriceDto, BundlePrice>, IBundlePriceService
    {
        private readonly IBundlePriceRepository _repository;
        public BundlePriceService(IBundlePriceRepository repository, IMapper mapper): base(repository,mapper) 
        {
            _repository = repository;
        }

        public Result<BundlePriceDto> Create(BundlePriceDto price)
        {
            var result = _repository.Create(new BundlePrice(price.Id,price.BundleId, price.TotalPrice));
            return MapToDto(result);
        }
        public Result<BundlePriceDto> GetPriceForBundle(long id)
        {
            var result = _repository.GetPriceById(id);
            return MapToDto(result);
        }
    }
}
