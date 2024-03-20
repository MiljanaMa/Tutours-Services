using System.Text.Json;
using System.Text.Json.Serialization;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public;
using Explorer.Encounters.Core.Domain;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Explorer.Tours.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Explorer.BuildingBlocks.Core.UseCases;

namespace Explorer.API.Controllers
{
    [Authorize(Policy = "userPolicy")]
    [Route("api/encounter")]
    public class EncounterController : BaseApiController
    {
        private readonly IEncounterService _encounterService;

        public EncounterController(IEncounterService encounterService)
        {
            _encounterService = encounterService;
        }

        private ActionResult<PagedResult<EncounterDto>> SendGetRequest(string url){
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(url).Result;

            if(response.IsSuccessStatusCode){
                string json = response.Content.ReadAsStringAsync().Result;
                List<EncounterDto> encounters = JsonSerializer.Deserialize<List<EncounterDto>>(json);
                PagedResult<EncounterDto> result = new PagedResult<EncounterDto>(encounters, encounters.Count);

                return Ok(result); 

            }

            return BadRequest($"Error: {response.StatusCode} - {response.ReasonPhrase}");
        

        }

        [HttpGet]
        public ActionResult<PagedResult<EncounterDto>> GetApproved([FromQuery] int page, [FromQuery] int pageSize)
        {
           return SendGetRequest("http://localhost:8083/encounters");
        }

        [HttpPost]
        public ActionResult<EncounterDto> Create([FromBody] EncounterDto encounter)
        {
            encounter.UserId = User.PersonId();
            var result = _encounterService.Create(encounter);
            return CreateResponse(result);
        }

        [HttpPut("{id:int}")]
        public ActionResult<EncounterDto> Update([FromBody] EncounterDto encounter)
        {
            var result = _encounterService.Update(encounter);
            return CreateResponse(result);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            string url = "http://localhost:8083/encounters/" + id; 
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.DeleteAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                return Ok("Success"); 
            }

            return BadRequest($"Error: {response.StatusCode} - {response.ReasonPhrase}"); 
        }

        [HttpGet("status")]
        public ActionResult<PagedResult<EncounterDto>> GetApprovedByStatus([FromQuery] int page, [FromQuery] int pageSize, [FromQuery] string status)
        {
            var result = _encounterService.GetApprovedByStatus(page, pageSize, status);
            return CreateResponse(result);
        }        
        
        [HttpGet("nearbyHidden")]
        public ActionResult<PagedResult<EncounterDto>> GetNearbyHidden([FromQuery] int page, [FromQuery] int pageSize)
        {
            var userId = ClaimsPrincipalExtensions.PersonId(User);
            return SendGetRequest("http://localhost:8083/encounters/nearby-by-type/"+userId);
           
        }

        [HttpGet("nearby")]
        public ActionResult<PagedResult<EncounterDto>> GetNearby([FromQuery] int page, [FromQuery] int pageSize)
        {
            var userId = ClaimsPrincipalExtensions.PersonId(User);
            return SendGetRequest("http://localhost:8083/encounters/nearby/"+userId);

        }

        [HttpGet("byUser")]
        public ActionResult<PagedResult<EncounterDto>> GetByUser([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _encounterService.GetByUser(page, pageSize, ClaimsPrincipalExtensions.PersonId(User));
            return CreateResponse(result);
        }

        [HttpPut("approve")]
        [Authorize(Roles = "administrator")]
        public ActionResult<EncounterDto> Approve(EncounterDto encounter)
        {
            var result = _encounterService.Approve(encounter);
            return CreateResponse(result);
        }

        [HttpPut("decline")]
        [Authorize(Roles = "administrator")]
        public ActionResult<EncounterDto> Decline(EncounterDto encounter)
        {
            var result = _encounterService.Decline(encounter);
            return CreateResponse(result);
        }

        [HttpGet("touristCreatedEncouters")]
        public ActionResult<PagedResult<EncounterDto>> GetTouristCreatedEncounters([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _encounterService.GetTouristCreatedEncounters(page, pageSize);
            return CreateResponse(result);
        }
        
    }
}
