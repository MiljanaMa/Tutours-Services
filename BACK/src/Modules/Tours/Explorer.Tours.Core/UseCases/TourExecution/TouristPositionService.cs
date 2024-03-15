using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Internal;
using Explorer.Tours.API.Public.TourExecution;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using FluentResults;

namespace Explorer.Tours.Core.UseCases.TourExecution;

public class TouristPositionService : CrudService<TouristPositionDto, TouristPosition>, ITouristPositionService, IInternalTouristPositionService
{
    protected readonly ITouristPositionRepository _touristPositionRepository;

    public TouristPositionService(ITouristPositionRepository repository, IMapper mapper) : base(repository, mapper)
    {
        _touristPositionRepository = repository;
    }

    public Result<TouristPositionDto> GetByUser(long userId)
    {
        try
        {
            var result = _touristPositionRepository.GetByUser(userId);
            return MapToDto(result);
        }
        catch (KeyNotFoundException e)
        {
            return Result.Fail(FailureCode.NotFound).WithError(e.Message);
        }
    }

    public override Result<TouristPositionDto> Create(TouristPositionDto entity)
    {
        try
        {
            var existingTouristPosition = _touristPositionRepository.GetByUser(entity.UserId);
            return Result.Fail(FailureCode.Conflict).WithError("Tourist position for this user already exists");
        }
        catch (ArgumentException e)
        {
            return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
        }
        catch (KeyNotFoundException e)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            var result = _touristPositionRepository.Create(MapToDomain(entity));
            return MapToDto(result);
        }
    }

    public override Result<TouristPositionDto> Update(TouristPositionDto entity)
    {
        try
        {
            var foundTouristPosition = _touristPositionRepository.GetByUser(entity.UserId);
            entity.Id = foundTouristPosition.Id;
            entity.UpdatedAt = DateTime.UtcNow;
            return base.Update(entity);
        }
        catch (KeyNotFoundException e)
        {
            return Result.Fail(FailureCode.NotFound).WithError(e.Message);
        }
    }
}