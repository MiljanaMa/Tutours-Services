using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public;
using Explorer.Encounters.Core.Domain;
using Explorer.Encounters.Core.UseCases;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Explorer.Tours.Core.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace Explorer.API.Controllers.Author
{
    [Route("api/author/encounter")]
    public class KeypointEncounterController : BaseApiController
    {
        private readonly IKeypointEncounterService _keypointEncounterService;
        protected static HttpClient httpClient = new()
        {
            BaseAddress = new Uri($"http://{Environment.GetEnvironmentVariable("ENCOUNTER_HOST") ?? "localhost"}:{Environment.GetEnvironmentVariable("ENCOUNTER_PORT") ?? "8083"}/keypointencounter/")
        };

        public KeypointEncounterController(IKeypointEncounterService encounterService)
        {
            _keypointEncounterService = encounterService;
        }
        
        [HttpGet("{keypointId:long}")]
        [Authorize(Roles = "author, tourist")]
        public ActionResult<PagedResult<KeypointEncounterDto>> GetPagedByKeypoint([FromQuery] int page, [FromQuery] int pageSize, long keypointId)
        {
            //string url = $"http://localhost:8083/keypointencounter/{keypointId}";

            //HttpClient client = new HttpClient();
            HttpResponseMessage response = httpClient.GetAsync($"{keypointId}").Result;
            if (response.IsSuccessStatusCode)
            {
                string json = response.Content.ReadAsStringAsync().Result;
                List<KeypointEncounterDto> keyPointEncounters = JsonSerializer.Deserialize<List<KeypointEncounterDto>>(json);
                PagedResult<KeypointEncounterDto> result = new PagedResult<KeypointEncounterDto>(keyPointEncounters, keyPointEncounters.Count);

                return Ok(result);
            }

            return BadRequest($"Error: {response.StatusCode} - {response.ReasonPhrase}");

        }

        [HttpPost]
        [Authorize(Roles = "author")]
        public ActionResult<KeypointEncounterDto> Create([FromBody] KeypointEncounterDto keypointEncounter)
        {
            //string url = "http://localhost:8083/keypointencounter/create";

            //using (HttpClient client = new HttpClient())
            //{
                string jsonContent = JsonSerializer.Serialize(keypointEncounter);
                HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage response = httpClient.PostAsync("create", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = response.Content.ReadAsStringAsync().Result;
                    KeypointEncounterDto createdKeypointEncounter = JsonSerializer.Deserialize<KeypointEncounterDto>(jsonResponse);
                    return Ok(createdKeypointEncounter);
                }
                else
                {
                    return BadRequest($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            //}
        }

        [HttpPut]
        [Authorize(Roles = "author")]
        public ActionResult<KeypointEncounterDto> Update([FromBody] KeypointEncounterDto keypointEncounter)
        {
            //string url = "http://localhost:8083/keypointencounter/update";

            //using (HttpClient client = new HttpClient())
            //{
                string jsonContent = JsonSerializer.Serialize(keypointEncounter);
                HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage response = httpClient.PutAsync("update", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = response.Content.ReadAsStringAsync().Result;
                    KeypointEncounterDto updatedKeypointEncounter = JsonSerializer.Deserialize<KeypointEncounterDto>(jsonResponse);
                    return Ok(updatedKeypointEncounter);
                }
                else
                {
                    return BadRequest($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            //}
        }

        //[HttpDelete("{id:int}")]
        [HttpDelete("/deletekeypointenc")]
        [Authorize(Roles = "author")]
        public ActionResult Delete([FromQuery] string id)
        {
            //string url = $"http://localhost:8083/keypointencounter/delete?id={id}";

            //using (HttpClient client = new HttpClient())
            //{
                HttpResponseMessage response = httpClient.DeleteAsync($"delete?id={id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    return NoContent();
                }
                else
                {
                    return BadRequest($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            //}
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
