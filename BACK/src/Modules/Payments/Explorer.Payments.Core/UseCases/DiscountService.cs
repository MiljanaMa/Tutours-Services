using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public;
using Explorer.Payments.Core.Domain;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;
using FluentResults;

namespace Explorer.Payments.Core.UseCases;

public class DiscountService:CrudService<DiscountDto, Discount>, IDiscountService
{
    private readonly IDiscountRepository _discountRepository;

    public DiscountService(IDiscountRepository discountRepository, IMapper mapper) : base(discountRepository, mapper)
    {
        _discountRepository = discountRepository;
    }

    public Result<double> GetDiscountForTour(int tourId)
    {
        return _discountRepository.GetDiscountForTour(tourId);
    }

    public Result<PagedResult<DiscountDto>> GetDiscountsByAuthor(int userId, int page, int pageSize)
    {
        var result = _discountRepository.GetDiscountsByAuthor(userId, page, pageSize);
        return MapToDto(result);
    }

    public Result<PagedResult<DiscountDto>> GetAllWithTours(int page, int pageSize)
    {
        var result = _discountRepository.GetAllWithTours(page, pageSize);
        return MapToDto(result);
    }

}