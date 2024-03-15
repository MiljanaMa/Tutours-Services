using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.MarketPlace;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.UseCases.MarketPlace
{
    public class BundleService : CrudService<BundleDto, Bundle>, IBundleService
    {
        private readonly IBundleRepository _repository;
        private readonly ITourRepository _tourRepository;
        public BundleService(IBundleRepository repository,ITourRepository tourRepository, IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
            _tourRepository = tourRepository;
        }

        public Result<BundleDto> Create(BundleDto bundle)
        {
            var result = _repository.Create(new Bundle(bundle.Id, bundle.Name, bundle.Status));
            return MapToDto(result);
        }


        public Result<PagedResult<BundleDto>> GetPaged(int page, int pageSize)
        {
            var result = _repository.GetPaged(page, pageSize);
            return MapToDto(result);
        }

        public Result<double> CalculatePrice(long bundleId)
        {
            double sum = 0;
            var result = _repository.getFullBundle(bundleId);
            foreach (var item in result.Tours)
            {
                sum += item.Price;
            }

            return sum;
        }

        public Result<BundleDto> Get(long id)
        {
            var result = _repository.getFullBundle(id);
            return MapToDto(result);
        }
        Result<BundleDto> Update(BundleDto newBundle)
        {
            var existingTour = _repository.Get(newBundle.Id);
            return MapToDto(_repository.Update(existingTour));
        }

        Result Delete(int id)
        {
            try
            {
                _repository.Delete(id);
                return Result.Ok();
            }
            catch (KeyNotFoundException e)
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
        }

        public Result<BundleDto> AddTourToBundle(long tourId, long bundleId)
        {
            var tour = _tourRepository.Get(tourId);
            var bundle = _repository.Get(bundleId);

            if(tour!= null && bundle !=null)
            {
                bundle.Tours.Add(tour);
                _repository.Update(bundle);
            }
            
            return MapToDto(bundle);
        }

        public Result RemoveTourFromBundle(long tourId, long bundleId)
        {
            var tour = _tourRepository.Get(tourId);
            var bundle = _repository.getFullBundle(bundleId);

           
            bundle.Tours.Remove(tour);
            _repository.Update(bundle);

            return Result.Ok();
           
        }

        
        public Result<BundleDto> PublishBundle(long bundleId)
        {
           
            var result = _repository.getFullBundle(bundleId);
            result.Publish(result);
            _repository.Update(result);
            return MapToDto(result);
                
        }

        public Result<BundleDto> ArchiveBundle(long bundleId)
        {
            var result = _repository.getFullBundle(bundleId);
            result.Archive(result);
            _repository.Update(result);
            return MapToDto(result);
        }
       
    }
}
