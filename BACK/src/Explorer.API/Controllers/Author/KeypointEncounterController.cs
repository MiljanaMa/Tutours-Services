using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public;
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

        public KeypointEncounterController(IKeypointEncounterService encounterService)
        {
            _keypointEncounterService = encounterService;
        }

        [HttpGet("{keypointId:long}")]
        [Authorize(Roles = "author, tourist")]
        public ActionResult<PagedResult<KeypointEncounterDto_1>> GetPagedByKeypoint([FromQuery] int page, [FromQuery] int pageSize, long keypointId)
        {
            string url = $"http://localhost:8083/keypointencounter/{keypointId}";

            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                string json = response.Content.ReadAsStringAsync().Result;
                List<KeypointEncounterDto_1> keyPointEncounters = JsonSerializer.Deserialize<List<KeypointEncounterDto_1>>(json);
                PagedResult<KeypointEncounterDto_1> result = new PagedResult<KeypointEncounterDto_1>(keyPointEncounters, keyPointEncounters.Count);

                return Ok(result);
            }

            return BadRequest($"Error: {response.StatusCode} - {response.ReasonPhrase}");

        }

        [HttpPost]
        [Authorize(Roles = "author")]
        public ActionResult<KeypointEncounterDto_1> Create([FromBody] KeypointEncounterDto_1 keypointEncounter)
        {
            string url = "http://localhost:8083/keypointencounter/create";

            using (HttpClient client = new HttpClient())
            {
                string jsonContent = JsonSerializer.Serialize(keypointEncounter);
                HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage response = client.PostAsync(url, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = response.Content.ReadAsStringAsync().Result;
                    KeypointEncounterDto_1 createdKeypointEncounter = JsonSerializer.Deserialize<KeypointEncounterDto_1>(jsonResponse);
                    return Ok(createdKeypointEncounter);
                }
                else
                {
                    return BadRequest($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
        }

        [HttpPut]
        [Authorize(Roles = "author")]
        public ActionResult<KeypointEncounterDto_1> Update([FromBody] KeypointEncounterDto_1 keypointEncounter)
        {
            string url = "http://localhost:8083/keypointencounter/update";

            using (HttpClient client = new HttpClient())
            {
                string jsonContent = JsonSerializer.Serialize(keypointEncounter);
                HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage response = client.PutAsync(url, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = response.Content.ReadAsStringAsync().Result;
                    KeypointEncounterDto_1 updatedKeypointEncounter = JsonSerializer.Deserialize<KeypointEncounterDto_1>(jsonResponse);
                    return Ok(updatedKeypointEncounter);
                }
                else
                {
                    return BadRequest($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            /*var result = _keypointEncounterService.Update(keypointEncounter);
            return CreateResponse(result);*/
        }

        //[HttpDelete("{id:int}")]
        [HttpDelete("/deletekeypointenc")]
        [Authorize(Roles = "author")]
        public ActionResult Delete( string id) //UuidDto
        {
            string url = $"http://localhost:8083/keypointencounter/delete";

            using (HttpClient client = new HttpClient())
            {

                HttpResponseMessage response = client.DeleteAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    return NoContent();
                }
                else
                {
                    return BadRequest($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }

            /*var result = _keypointEncounterService.Delete(id);
            return CreateResponse(result);*/
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
