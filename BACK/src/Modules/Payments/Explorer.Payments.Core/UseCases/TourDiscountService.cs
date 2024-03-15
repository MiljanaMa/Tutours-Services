using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public;
using Explorer.Payments.Core.Domain;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;
using FluentResults;

namespace Explorer.Payments.Core.UseCases;

public class TourDiscountService : CrudService<TourDiscountDto, TourDiscount>, ITourDiscountService
{
    private readonly ITourDiscountRepository _discountRepository;
    public TourDiscountService(ITourDiscountRepository tourDiscountRepository, IMapper mapper) : base(tourDiscountRepository, mapper)
    {
        _discountRepository = tourDiscountRepository;
    }
    public new Result<TourDiscountDto> Create(TourDiscountDto tourDiscountDto)
    {
        var tour = MapToDomain(tourDiscountDto);
        var result = _discountRepository.Create(tour);
        if (result == null)
            return Result.Fail("Tour is already on Discount");
        return MapToDto(result);
    }

    public new Result Delete(int tourId)
    {
        try
        {
            _discountRepository.Delete(tourId);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            return Result.Fail("Failed to delete the tour from the Discount.")
                .WithError(ex.Message);
        }
    }

    public Result<List<int>> GetToursFromOtherDiscounts(int discountId)
    {
        return _discountRepository.GetToursFromOtherDiscounts(discountId);
    }
}