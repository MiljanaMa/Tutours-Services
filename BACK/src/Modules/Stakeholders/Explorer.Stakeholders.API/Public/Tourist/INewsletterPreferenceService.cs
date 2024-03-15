using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Public.Tourist;

public interface INewsletterPreferenceService
{
    public Result<PagedResult<NewsletterPreferenceDto>> GetPaged(int page, int pageSize);
    public Result<NewsletterPreferenceDto> Get(int id);
    public Result<NewsletterPreferenceDto> Create(NewsletterPreferenceDto entity);
    public Result<NewsletterPreferenceDto> Update(NewsletterPreferenceDto entity);
    public List<NewsletterPreferenceDto> CheckCandidatesForNewsletter();
    public void SendNewsletter(List<NewsletterPreferenceDto> candidates);
    public Result Delete(int id);
}
