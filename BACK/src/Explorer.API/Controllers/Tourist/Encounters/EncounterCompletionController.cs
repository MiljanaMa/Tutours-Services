using Microsoft.AspNetCore.Authorization;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Microsoft.AspNetCore.Mvc;
using FluentResults;
using Explorer.Encounters.Core.Domain;
using System.Text.Json;

namespace Explorer.API.Controllers.Tourist.Encounters
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourist/encounter")]
    public class EncounterCompletionController : BaseApiController
    {
        private readonly IEncounterCompletionService _encounterCompletionService;
        protected static HttpClient httpClient = new()
        {
            BaseAddress = new Uri($"http://{Environment.GetEnvironmentVariable("ENCOUNTER_HOST") ?? "localhost"}:{Environment.GetEnvironmentVariable("ENCOUNTER_PORT") ?? "8083"}/tourist/encounter/")
        };

        public EncounterCompletionController(IEncounterCompletionService encounterCompletionService)
        {
            _encounterCompletionService = encounterCompletionService;
        }
        
        [HttpGet]
        public ActionResult<PagedResult<EncounterCompletionDto_1>> GetPagedByUser([FromQuery] int page, [FromQuery] int pageSize)
        {
            //string url = $"http://localhost:8083/tourist/encounter/{ClaimsPrincipalExtensions.PersonId(User)}";

            //HttpClient client = new HttpClient();
            HttpResponseMessage response = httpClient.GetAsync($"{ClaimsPrincipalExtensions.PersonId(User)}").Result;
            if (response.IsSuccessStatusCode)
            {
                string json = response.Content.ReadAsStringAsync().Result;
                List<EncounterCompletionDto_1> encounterCompletions = JsonSerializer.Deserialize<List<EncounterCompletionDto_1>>(json);
                PagedResult<EncounterCompletionDto_1> result = new PagedResult<EncounterCompletionDto_1>(encounterCompletions, encounterCompletions.Count);

                return Ok(result);
            }

            return BadRequest($"Error: {response.StatusCode} - {response.ReasonPhrase}");
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
            //var url = $"http://localhost:8083/tourist/encounter/finishEncounter/{userId}";
            
            //HttpClient client = new HttpClient();
            HttpResponseMessage response = httpClient.GetAsync($"finishEncounter/{userId}").Result;

            if (response.IsSuccessStatusCode)
            {
                var json = response.Content.ReadAsStringAsync().Result;
                var result = JsonSerializer.Deserialize<EncounterCompletionDto>(json);
                return Ok(result);
            }

            return BadRequest($"Error: {response.StatusCode} - {response.ReasonPhrase}");
        }

        /*
         public ActionResult<EncounterCompletionDto> FinishEncounter([FromBody] EncounterDto encounter)
        {
            var userId = ClaimsPrincipalExtensions.PersonId(User);

            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.PutAsJsonAsync(url, encounter).Result;

            if (response.IsSuccessStatusCode)
            {
                var json = response.Content.ReadAsStringAsync().Result;
                var result = JsonSerializer.Deserialize<EncounterCompletionDto>(json);
                return Ok(result);
            }

            return BadRequest($"Error: {response.StatusCode} - {response.ReasonPhrase}");

        }

        [HttpGet("checkNearbyCompletions")]
        public ActionResult<PagedResult<EncounterCompletionDto>> CheckNearbyEncounters() // currently handles only hidden encounters, it would be benefitial if all checks for nearby encounters would be here together with criteria for completition
        {
            var userId = ClaimsPrincipalExtensions.PersonId(User);
            var result = _encounterCompletionService.CheckNearbyEncounters(userId);
            return CreateResponse(result);
        }*/
    }
}
