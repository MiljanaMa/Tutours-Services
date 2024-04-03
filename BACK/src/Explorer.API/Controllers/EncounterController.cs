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
        protected static HttpClient httpClient = new()
        {
            BaseAddress = new Uri($"http://{Environment.GetEnvironmentVariable("ENCOUNTER_HOST") ?? "localhost"}:{Environment.GetEnvironmentVariable("ENCOUNTER_PORT") ?? "8083"}/encounters/")
        };

        public EncounterController(IEncounterService encounterService)
        {
            _encounterService = encounterService;
        }

        private ActionResult<PagedResult<EncounterDto>> SendGetRequest(string url){
            HttpResponseMessage response = httpClient.GetAsync(url).Result;
            
            if(response.IsSuccessStatusCode){
                string json = response.Content.ReadAsStringAsync().Result;
                List<EncounterDto> encounters = JsonSerializer.Deserialize<List<EncounterDto>>(json);
                PagedResult<EncounterDto> result; 
                if (encounters != null)
                {
                    result = new PagedResult<EncounterDto>(encounters, encounters.Count);
                }
                else
                {
                    result = new PagedResult<EncounterDto>(new List<EncounterDto>(), 0); 
                }
                

                
                return Ok(result); 

            }
            return BadRequest($"Error: {response.StatusCode} - {response.ReasonPhrase}");
        

        }

        [HttpGet]
        public ActionResult<PagedResult<EncounterDto>> GetApproved([FromQuery] int page, [FromQuery] int pageSize)
        {
           return SendGetRequest("");
        }

        [HttpPost]
        public ActionResult<EncounterDto> Create([FromBody] EncounterDto encounter)
        {
            encounter.UserId = User.PersonId();
            var result = _encounterService.Create(encounter);
            return CreateResponse(result);
        }
        

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            //string url = "http://localhost:8083/encounters/" + id; 
            //HttpClient client = new HttpClient();
            HttpResponseMessage response = httpClient.DeleteAsync($"{id}").Result;
            if (response.IsSuccessStatusCode)
            {
                return Ok("Success"); 
            }

            return BadRequest($"Error: {response.StatusCode} - {response.ReasonPhrase}"); 
        }

        [HttpGet("status")]
        public ActionResult<PagedResult<EncounterDto>> GetApprovedByStatus([FromQuery] int page, [FromQuery] int pageSize, [FromQuery] string status)
        {
            return SendGetRequest("get-approved-by-status/" + status);;
        }        
        
        [HttpGet("nearbyHidden")]
        public ActionResult<PagedResult<EncounterDto>> GetNearbyHidden([FromQuery] int page, [FromQuery] int pageSize)
        {
            var userId = ClaimsPrincipalExtensions.PersonId(User);
            return SendGetRequest("nearby-by-type/" + userId);
           
        }

        [HttpGet("nearby")] 
        public ActionResult<PagedResult<EncounterDto>> GetNearby([FromQuery] int page, [FromQuery] int pageSize)
        {
            var userId = ClaimsPrincipalExtensions.PersonId(User);
            return SendGetRequest("nearby");

        }

        [HttpGet("byUser")]
        public ActionResult<PagedResult<EncounterDto>> GetByUser([FromQuery] int page, [FromQuery] int pageSize)
        {
            var userId = ClaimsPrincipalExtensions.PersonId(User);
            return SendGetRequest("get-by-user/" + userId);
        }

        [HttpGet("touristCreatedEncouters")]
        public ActionResult<PagedResult<EncounterDto>> GetTouristCreatedEncounters([FromQuery] int page, [FromQuery] int pageSize)
        {
            return SendGetRequest("tourist-created-encounters");
        }
        
        [HttpPut("approve")]
        [Authorize(Roles = "administrator")]
        public ActionResult<EncounterDto> Approve(EncounterDto encounter)
        {
            return sendPut("approve", encounter); 
        }

        [HttpPut("decline")]
        [Authorize(Roles = "administrator")]
        public ActionResult<EncounterDto> Decline(EncounterDto encounter)
        {
            return sendPut("decline", encounter); 
        }

        [HttpPut("{encounterId:int}")]
        public ActionResult<EncounterDto> Update([FromRoute] int encounterId, [FromBody] EncounterDto encounter)
        {
            return sendPut("", encounter); 
        }

        private ActionResult<EncounterDto> sendPut(string url, EncounterDto encounter)
        {
            var jsonEncounter = JsonSerializer.Serialize(encounter);

            Console.WriteLine("JSON:",jsonEncounter);
            //HttpClient client = new HttpClient();

            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(
            //    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = httpClient.PutAsync(url,
                new StringContent(jsonEncounter, System.Text.Encoding.UTF8, "application/json")).Result;

            if (response.IsSuccessStatusCode)
            {
                string json = response.Content.ReadAsStringAsync().Result;
                EncounterDto result = JsonSerializer.Deserialize<EncounterDto>(json);

                return Ok(result);

            }

            return BadRequest(response.ReasonPhrase); 
        }
        
        
    }
}
