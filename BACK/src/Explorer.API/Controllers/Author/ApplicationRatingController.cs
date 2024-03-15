using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Author;

[Authorize(Policy = "authorPolicy")]
[Route("api/author/appRating")]
public class ApplicationRatingController : BaseApiController
{
    private readonly IApplicationRatingService _applicationRatingService;

    public ApplicationRatingController(IApplicationRatingService applicationRatingService)
    {
        _applicationRatingService = applicationRatingService;
    }

    [HttpPost]
    public ActionResult<ApplicationRatingDto> Create([FromBody] ApplicationRatingDto applicationRatingDto)
    {
        applicationRatingDto.UserId = User.PersonId();

        var result = _applicationRatingService.Create(applicationRatingDto);
        return CreateResponse(result);
    }

    [HttpPut]
    public ActionResult<ApplicationRatingDto> Update([FromBody] ApplicationRatingDto applicationRatingDto)
    {
        applicationRatingDto.UserId = User.PersonId();
        var result = _applicationRatingService.Update(applicationRatingDto);
        return CreateResponse(result);
    }

    [HttpGet]
    public ActionResult<ApplicationRatingDto> GetByUser()
    {
        var userId = User.PersonId();
        var result = _applicationRatingService.GetByUser(userId);
        return CreateResponse(result);
    }

    [HttpDelete]
    public ActionResult Delete()
    {
        var userId = User.PersonId();
        var result = _applicationRatingService.Delete(userId);
        return CreateResponse(result);
    }
}