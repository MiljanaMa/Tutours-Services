using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Dtos.Enums;
using Explorer.Stakeholders.API.Internal;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using FluentResults;

namespace Explorer.Stakeholders.Core.UseCases;

public class NotificationService : BaseService<NotificationDto, Notification>, INotificationService, IInternalNotificationService
{
    protected readonly INotificationRepository _notificationRepository;

    public NotificationService(INotificationRepository repository, IMapper mapper) : base(mapper)
    {
        _notificationRepository = repository;
    }

    public Result<PagedResult<NotificationDto>> GetByUser(int page, int pageSize, int userId)
    {
        var result = _notificationRepository.GetByUser(page, pageSize, userId);
        return MapToDto(result);
    }

    public void Generate(int userId, NotificationType typeDto, string actionURL, DateTime date,
        string additionalMessage)
    {
        var typeDomain = (Domain.Enums.NotificationType)typeDto;
        var notification = new Notification(userId, typeDomain, actionURL, date, false, additionalMessage);
        _notificationRepository.Generate(notification);
    }

    public Result MarkAsRead(int id)
    {
        try
        {
            _notificationRepository.MarkAsRead(id);
            return Result.Ok();
        }
        catch (KeyNotFoundException e)
        {
            return Result.Fail(FailureCode.NotFound).WithError(e.Message);
        }
    }

    public Result Delete(int id)
    {
        try
        {
            _notificationRepository.Delete(id);
            return Result.Ok();
        }
        catch (KeyNotFoundException e)
        {
            return Result.Fail(FailureCode.NotFound).WithError(e.Message);
        }
    }
}