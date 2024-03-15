using Microsoft.AspNetCore.Authorization;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Microsoft.AspNetCore.Mvc;
using FluentResults;

namespace Explorer.API.Controllers.Tourist.Encounters
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourist/encounter")]
    public class EncounterCompletionController : BaseApiController
    {
        private readonly IEncounterCompletionService _encounterCompletionService;

        public EncounterCompletionController(IEncounterCompletionService encounterCompletionService)
        {
            _encounterCompletionService = encounterCompletionService;
        }

        [HttpGet]
        public ActionResult<PagedResult<EncounterCompletionDto>> GetPagedByUser([FromQuery] int page, [FromQuery] int pageSize)
        {
            var userId = ClaimsPrincipalExtensions.PersonId(User);
            var result = _encounterCompletionService.GetPagedByUser(page, pageSize, userId);
            return CreateResponse(result);
        }

        [HttpPost("completions")]
        public ActionResult<List<EncounterCompletionDto>> GetByIds([FromBody] List<int> ids)
        {
            var result = _encounterCompletionService.GetByIds(ids);
            return CreateResponse(result);
        }


        [HttpPost("updateSocialEncounters")]
        public ActionResult UpdateSocialEncounters()
        {
            _encounterCompletionService.UpdateSocialEncounters();
            return CreateResponse(Result.Ok());
        }
        [HttpPost("startEncounter")]
        public ActionResult<EncounterCompletionDto> StartEncounter([FromBody] EncounterDto encounter)
        {
            var userId = ClaimsPrincipalExtensions.PersonId(User);
            var result = _encounterCompletionService.StartEncounter(userId, encounter);
            return CreateResponse(result);
        }
        [HttpPut("finishEncounter")]
        public ActionResult<EncounterCompletionDto> FinishEncounter([FromBody] EncounterDto encounter)
        {
            var userId = ClaimsPrincipalExtensions.PersonId(User);
            var result = _encounterCompletionService.FinishEncounter(userId, encounter);
            return CreateResponse(result);
        }

        [HttpGet("checkNearbyCompletions")]
        public ActionResult<PagedResult<EncounterCompletionDto>> CheckNearbyEncounters() // currently handles only hidden encounters, it would be benefitial if all checks for nearby encounters would be here together with criteria for completition
        {
            var userId = ClaimsPrincipalExtensions.PersonId(User);
            var result = _encounterCompletionService.CheckNearbyEncounters(userId);
            return CreateResponse(result);
        }
    }
}
