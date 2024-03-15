using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Public.Blog;
using Explorer.BuildingBlocks.Core.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist.Blog;

[Authorize(Policy = "touristPolicy")]
[Route("api/blog/blogStatus")]
public class BlogStatusController : BaseApiController
{
    private readonly IBlogStatusService _blogStatusService;

    public BlogStatusController(IBlogStatusService blogStatusService)
    {
        _blogStatusService = blogStatusService;
    }

    [HttpGet]
    public ActionResult<PagedResult<BlogStatusDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
    {
        var result = _blogStatusService.GetPaged(page, pageSize);
        return CreateResponse(result);
    }

    [HttpGet("{id:int}")]
    public ActionResult<BlogStatusDto> Get(int id)
    {
        var result = _blogStatusService.Get(id);
        return CreateResponse(result);
    }

    [HttpPost]
    public ActionResult<BlogStatusDto> Create([FromBody] BlogStatusDto blogStatus)
    {
        var result = _blogStatusService.Create(blogStatus);
        return CreateResponse(result);
    }

    [HttpPut("{id:int}")]
    public ActionResult<BlogStatusDto> Update([FromBody] BlogStatusDto blogStatus)
    {
        var result = _blogStatusService.Update(blogStatus);
        return CreateResponse(result);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var result = _blogStatusService.Delete(id);
        return CreateResponse(result);
    }
}