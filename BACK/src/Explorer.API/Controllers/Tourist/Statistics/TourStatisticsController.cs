using Explorer.Encounters.API.Dtos;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Statistics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist.Statistics
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourist/tourStatistics")]
    public class TourStatisticsController : BaseApiController
    {
        private readonly IStatisticsService _statisticsService;

        public TourStatisticsController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        [HttpGet("yearCompletions")]
        public ActionResult<TourYearStatsDto> GetByUserAndYear([FromQuery] int year)
        {
            var userId = ClaimsPrincipalExtensions.PersonId(User);
            var result = _statisticsService.GetEncounterYearStatsByUser(userId, year);
            return CreateResponse(result);
        }
    }
}
