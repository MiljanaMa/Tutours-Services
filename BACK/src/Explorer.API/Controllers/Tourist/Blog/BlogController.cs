using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Public.Blog;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.Infrastructure.Authentication;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist.Blog;

[Authorize(Policy = "touristPolicy")]
[Route("api/blog")]
public class BlogController : BaseApiController
{
    private readonly IBlogService _blogService;

    public BlogController(IBlogService blogService)
    {
        _blogService = blogService;
    }
    protected static HttpClient httpClient = new()
    {
        BaseAddress = new Uri($"http://localhost:8095/followers/")
    };

    [HttpGet]
    public ActionResult<PagedResult<BlogDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
    {
        var result = _blogService.GetPaged(page, pageSize);
        return CreateResponse(result);
    }

    [HttpGet("status")]
    public ActionResult<PagedResult<BlogDto>> GetWithStatuses([FromQuery] int page, [FromQuery] int pageSize)
    {
        var result = _blogService.GetWithStatuses(page, pageSize);
        return CreateResponse(result.ToResult());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BlogDto>> Get(int id)
    {
        var result = _blogService.Get(id);
        int id1 = result.Value.CreatorId;
        int id2 = User.PersonId();
        HttpResponseMessage response = await httpClient.GetAsync("check-if-following/"+id2+"/"+id1);
        if (response.IsSuccessStatusCode)
        {
            
            string responseContent = await response.Content.ReadAsStringAsync();
            bool isFollowing = bool.Parse(responseContent);
        
            if (isFollowing)
            {
                return CreateResponse(result);
            }
            else
            {
                return BadRequest("You are not following this creator.");
            }
            
        }
       
        
        return BadRequest($"Error: {response.StatusCode} - {response.ReasonPhrase}");
    }

    [HttpPost]
    public ActionResult<BlogDto> Create([FromBody] BlogDto blog)
    {
        var result = _blogService.Create(blog);
        return CreateResponse(result);
    }

    [HttpPut("{id:int}")]
    public ActionResult<BlogDto> Update([FromBody] BlogDto blog)
    {
        var result = _blogService.Update(blog);
        return CreateResponse(result);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var result = _blogService.Delete(id);
        return CreateResponse(result);
    }

    [HttpPost("rate")]
    public ActionResult<BlogDto> AddRating([FromBody] BlogRatingDto blogRatingDto)
    {
        var result = _blogService.AddRating(blogRatingDto, User.PersonId());
        return CreateResponse(result);
    }
}