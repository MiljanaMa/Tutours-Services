using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public;
using Explorer.Encounters.Core.UseCases;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Explorer.Tours.Core.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Author
{
    [Route("api/author/encounter")]
    public class KeypointEncounterController : BaseApiController
    {
        private readonly IKeypointEncounterService _keypointEncounterService;

        public KeypointEncounterController(IKeypointEncounterService encounterService)
        {
            _keypointEncounterService = encounterService;
        }

        [HttpGet("{keypointId:long}")]
        [Authorize(Roles = "author, tourist")]
        public ActionResult<PagedResult<EncounterCompletionDto>> GetPagedByKeypoint([FromQuery] int page, [FromQuery] int pageSize, long keypointId)
        {
            var result = _keypointEncounterService.GetPagedByKeypoint(page, pageSize, keypointId);
            return CreateResponse(result);
        }
        [HttpPost]
        [Authorize(Roles = "author")]
        public ActionResult<KeypointEncounterDto> Create([FromBody] KeypointEncounterDto keypointEncounter)
        {
            keypointEncounter.Encounter.UserId = User.PersonId();
            var result = _keypointEncounterService.Create(keypointEncounter);
            return CreateResponse(result);
        }

        [HttpPut]
        [Authorize(Roles = "author")]
        public ActionResult<KeypointEncounterDto> Update([FromBody] KeypointEncounterDto keypointEncounter)
        {
            var result = _keypointEncounterService.Update(keypointEncounter);
            return CreateResponse(result);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "author")]
        public ActionResult Delete(int id)
        {
            var result = _keypointEncounterService.Delete(id);
            return CreateResponse(result);
        }
        [HttpPut("{keypointId:int}")]
        [Authorize(Roles = "author")]
        public ActionResult<KeypointEncounterDto> UpdateEncounterLocation([FromBody] LocationDto location, int keypointId)
        {
            var result  = _keypointEncounterService.UpdateEncountersLocation(location, keypointId);
            return CreateResponse(result);
        }
        [HttpDelete("keypoint/{keypointId:int}")]
        [Authorize(Roles = "author")]
        public ActionResult DeleteKeypointEncounters(int keypointId)
        {
            var result = _keypointEncounterService.DeleteKeypointEncounters(keypointId);
            return CreateResponse(result);
        }
    }
}
