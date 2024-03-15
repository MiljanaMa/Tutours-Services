using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Dtos.Enums;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.API.Public.TourExecution;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Explorer.Tours.API.Internal;
using FluentResults;

namespace Explorer.Stakeholders.Core.UseCases.TourExecution;

public class TourIssueService : CrudService<TourIssueDto, TourIssue>, ITourIssueService
{
    private readonly INotificationService _notificationService;
    private readonly ITourIssueRepository _repo;
    private readonly IInternalTourService _tourService;
    private readonly IUserService _userService;

    public TourIssueService(ITourIssueRepository crudRepository, IMapper mapper,
        INotificationService notificationService, IUserService userService,
        IInternalTourService tourService) : base(crudRepository, mapper)
    {
        _repo = crudRepository;
        _notificationService = notificationService;
        _userService = userService;
        _tourService = tourService;
    }

    public Result<PagedResult<TourIssueDto>> GetByUserPaged(int page, int pageSize, int id)
    {
        var result = _repo.GetByUserPaged(page, pageSize, id);
        return MapToDto(result);
    }

    public Result<PagedResult<TourIssueDto>> GetByTourId(int page, int pageSize, int tourId)
    {
        var result = _repo.GetByTourId(page, pageSize, tourId);
        return MapToDto(result);
    }

    public Result<PagedResult<TourIssueDto>> GetByTourIssueId(int page, int pageSize, int tourIssueId)
    {
        var result = _repo.GetByTourIssueId(page, pageSize, tourIssueId);
        return MapToDto(result);
    }

    public Result<TourIssueDto> SetResolvedDateTime(TourIssueDto tourIssue)
    {
        try
        {
            var result = CrudRepository.Update(MapToDomain(tourIssue));

            var tour = _tourService.Get(tourIssue.TourId).Value;
            var url = "/tourissue/" + tourIssue.Id;
            var additionalMessage = string.Format("{0:dd/MM/yyyy}", tourIssue.ResolveDateTime) +
                                    " for the tour named '" + tour.Name + "'.";

            _notificationService.Generate(tour.UserId, NotificationType.ISSUE_DEADLINE, url, DateTime.UtcNow,
                additionalMessage);

            return MapToDto(result);
        }
        catch (KeyNotFoundException e)
        {
            return Result.Fail(FailureCode.NotFound).WithError(e.Message);
        }
        catch (ArgumentException e)
        {
            return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
        }
    }
}