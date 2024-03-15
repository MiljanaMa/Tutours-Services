using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers;

[Authorize(Policy = "userPolicy")]
[Route("api/notification")]
public class NotificationController : BaseApiController
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpGet("byUser")]
    public ActionResult<PagedResult<NotificationDto>> GetByUser([FromQuery] int page, [FromQuery] int pageSize)
    {
        var userId = User.PersonId();
        var result = _notificationService.GetByUser(page, pageSize, userId);
        return CreateResponse(result);
    }

    [HttpPut("{id:int}")]
    public ActionResult MarkAsRead(int id)
    {
        var result = _notificationService.MarkAsRead(id);
        return CreateResponse(result);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var result = _notificationService.Delete(id);
        return CreateResponse(result);
    }
}