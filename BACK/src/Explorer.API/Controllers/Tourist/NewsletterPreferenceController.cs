using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public.Tourist;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist;

[Authorize(Policy = "userpolicy")]
[Route("api/tourist/newsletterpreference")]

public class NewsletterPreferenceController : BaseApiController
{
    private readonly INewsletterPreferenceService _newsletterPreferenceService;

    public NewsletterPreferenceController(INewsletterPreferenceService newsletterPreferenceService)
    {
        _newsletterPreferenceService = newsletterPreferenceService;
    }

    [HttpGet]
    public ActionResult<PagedResult<NewsletterPreferenceDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
    {
        var result = _newsletterPreferenceService.GetPaged(page, pageSize);
        return CreateResponse(result);
    }

    [HttpGet("{id}")]
    public ActionResult<PagedResult<NewsletterPreferenceDto>> Get(int id)
    {
        var result = _newsletterPreferenceService.Get(id);
        return CreateResponse(result);
    }

    [HttpPost]
    public ActionResult<NewsletterPreferenceDto> Create([FromBody] NewsletterPreferenceDto newsletterPreference)
    {
        var result = _newsletterPreferenceService.Create(newsletterPreference);
        return CreateResponse(result);
    }

    [HttpPut("{id}")]
    public ActionResult<NewsletterPreferenceDto> Update([FromBody] NewsletterPreferenceDto newsletterPreference)
    {
        var result = _newsletterPreferenceService.Update(newsletterPreference);
        return CreateResponse(result);
    }

    [HttpDelete]
    public ActionResult Delete([FromQuery] int id) 
    {
        var result = _newsletterPreferenceService.Delete(id);
        return CreateResponse(result);
    }
}
