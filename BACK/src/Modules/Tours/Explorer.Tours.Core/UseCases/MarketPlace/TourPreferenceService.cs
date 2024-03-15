using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.MarketPlace;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using FluentResults;

namespace Explorer.Tours.Core.UseCases.MarketPlace;

public class TourPreferenceService : CrudService<TourPreferenceDto, TourPreference>, ITourPreferenceService
{
    protected readonly ITourPreferenceRepository _tourPreferenceRepository; // ??

    public TourPreferenceService(ITourPreferenceRepository repository, IMapper mapper) : base(repository, mapper)
    {
        _tourPreferenceRepository = repository;
    }

    public Result<TourPreferenceDto> GetByUser(int userId)
    {
        try
        {
            var result = _tourPreferenceRepository.GetByUser(userId);
            return MapToDto(result);
        }
        catch (KeyNotFoundException e)
        {
            return Result.Fail(FailureCode.NotFound).WithError(e.Message);
        }
    }

    public override Result<TourPreferenceDto> Create(TourPreferenceDto entity)
    {
        try
        {
            var existingTourPreference = _tourPreferenceRepository.GetByUser(entity.UserId);
            return Result.Fail(FailureCode.Conflict).WithError("Tour preference for this user already exists");
        }
        catch (ArgumentException e)
        {
            return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
        }
        catch (KeyNotFoundException e)
        {
            var result = CrudRepository.Create(MapToDomain(entity));
            return MapToDto(result);
        }
    }

    public override Result<TourPreferenceDto> Update(TourPreferenceDto entity)
    {
        try
        {
            var foundTourPreference = _tourPreferenceRepository.GetByUser(entity.UserId);
            entity.Id = foundTourPreference.Id;
            return base.Update(entity);
        }
        catch (KeyNotFoundException e)
        {
            return Result.Fail(FailureCode.NotFound).WithError(e.Message);
        }
    }

    public override Result Delete(int userId)
    {
        try
        {
            var id = (int)_tourPreferenceRepository.GetByUser(userId).Id;
            return base.Delete(id);
        }
        catch (KeyNotFoundException e)
        {
            return Result.Fail(FailureCode.NotFound).WithError(e.Message);
        }
    }
}