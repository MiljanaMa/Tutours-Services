using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using FluentResults;

namespace Explorer.Stakeholders.Core.UseCases;

public class ApplicationRatingService : CrudService<ApplicationRatingDto, ApplicationRating>, IApplicationRatingService
{
    protected readonly IApplicationRatingRepository _applicationRatingRepository;

    public ApplicationRatingService(IApplicationRatingRepository repository, IMapper mapper) : base(repository, mapper)
    {
        _applicationRatingRepository = repository;
    }

    public Result<ApplicationRatingDto> GetByUser(int userId)
    {
        try
        {
            var result = _applicationRatingRepository.GetByUser(userId);
            return MapToDto(result);
        }
        catch (KeyNotFoundException e)
        {
            return Result.Fail(FailureCode.NotFound).WithError(e.Message);
        }
    }

    public override Result<ApplicationRatingDto> Create(ApplicationRatingDto entity)
    {
        try
        {
            var existingApplicationRating = _applicationRatingRepository.GetByUser(entity.UserId);
            return Result.Fail(FailureCode.Conflict).WithError("Application rating for this user already exists.");
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

    public override Result<ApplicationRatingDto> Update(ApplicationRatingDto entity)
    {
        try
        {
            var foundApplicationRating = _applicationRatingRepository.GetByUser(entity.UserId);
            entity.Id = (int)foundApplicationRating.Id;
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
            var id = (int)_applicationRatingRepository.GetByUser(userId).Id;
            return base.Delete(id);
        }
        catch (KeyNotFoundException e)
        {
            return Result.Fail(FailureCode.NotFound).WithError(e.Message);
        }
    }
}