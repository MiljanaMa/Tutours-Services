using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public;
using Explorer.Encounters.Core.UseCases;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist.Statistics
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourist/encounterStatistics")]
    public class EncounterStatisticsController : BaseApiController
    {
        private readonly IStatisticsService _statisticsService;

        public EncounterStatisticsController(IStatisticsService statisticsService)
        {
           _statisticsService = statisticsService;
        }

        [HttpGet("completions")]
        public ActionResult<EncounterStatsDto> GetByUser()
        {
            var userId = ClaimsPrincipalExtensions.PersonId(User);
            var result = _statisticsService.GetEncounterStatsByUser(userId);
            return CreateResponse(result);
        }

        [HttpGet("yearCompletions")]
        public ActionResult<EncounterStatsDto> GetByUserAndYear([FromQuery] int year)
        {
            var userId = ClaimsPrincipalExtensions.PersonId(User);
            var result = _statisticsService.GetEncounterYearStatsByUser(userId, year);
            return CreateResponse(result);
        }

    }
}
