using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.TourAuthoring;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Author
{
    [Authorize(Policy = "personPolicy")]
    [Route("api/author/publicKeypoints")]
    public class PublicKeypointController : BaseApiController
    {
        private readonly IPublicKeypointService _publicKeypointService;

        public PublicKeypointController(IPublicKeypointService publicKeypointService)
        {
            _publicKeypointService = publicKeypointService;
        }

        [HttpGet]
        public ActionResult<PagedResult<PublicKeypointDto>> GetPaged([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _publicKeypointService.GetPaged(page, pageSize);
            return CreateResponse(result);
        }
    }
}
