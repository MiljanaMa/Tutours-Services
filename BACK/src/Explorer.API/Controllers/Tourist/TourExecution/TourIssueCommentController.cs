using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public.TourExecution;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist.TourExecution;

[Authorize(Policy = "userPolicy")]
[Route("api/tourexecution/tourissuecomment")]
public class TourIssueCommentController : BaseApiController
{
    private readonly ITourIssueCommentService _tourIssueCommentService;

    public TourIssueCommentController(ITourIssueCommentService tourIssueCommentService)
    {
        _tourIssueCommentService = tourIssueCommentService;
    }

    private int GenerateId()
    {
        return _tourIssueCommentService.GetPaged(0, 0).Value.Results.Count == 0
            ? 1
            : _tourIssueCommentService.GetPaged(0, 0).Value.Results.Max(ti => ti.Id) + 1;
    }

    [HttpGet]
    public ActionResult<PagedResult<TourIssueCommentDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
    {
        var result = _tourIssueCommentService.GetPaged(page, pageSize);
        return CreateResponse(result);
    }

    [HttpGet("{id:int}")]
    public ActionResult<TourIssueCommentDto> Get(int id)
    {
        var result = _tourIssueCommentService.Get(id);
        return CreateResponse(result);
    }

    [HttpPost]
    public ActionResult<TourIssueCommentDto> Create([FromBody] TourIssueCommentDto comment)
    {
        comment.Id = GenerateId();
        comment.UserId = User.PersonId();
        var result = _tourIssueCommentService.Create(comment);
        return CreateResponse(result);
    }

    [HttpPut("{id:int}")]
    public ActionResult<TourIssueCommentDto> Update([FromBody] TourIssueCommentDto comment)
    {
        var result = _tourIssueCommentService.Update(comment);
        return CreateResponse(result);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var result = _tourIssueCommentService.Delete(id);
        return CreateResponse(result);
    }
}