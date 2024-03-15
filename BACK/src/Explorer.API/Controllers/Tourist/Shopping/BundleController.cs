using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.MarketPlace;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist.Shopping
{
    [Authorize(Policy = "authorPolicy")]
    [Route("api/bundles")]
    public class BundleController : BaseApiController
    {
        private readonly IBundleService _service;

        public BundleController(IBundleService service)
        {
            _service = service;
        }

        [HttpPost]
        public ActionResult<BundleDto> Create([FromBody]BundleDto bundle)
        {
            var result = _service.Create(bundle);
            return CreateResponse(result);
        }
        [HttpGet]
        public ActionResult<PagedResult<BundleDto>> GetPaged([FromQuery] int page, [FromQuery] int pageSize) 
        {
            var result = _service.GetPaged(page, pageSize);
            return CreateResponse(result);
        }

        [HttpGet("{id:int}")]
        public ActionResult<BundleDto> Get(long id)
        {
            var result = _service.Get(id);
            return CreateResponse(result);
        }

        [HttpPut]
        public ActionResult<BundleDto> Update ([FromBody ] BundleDto  bundle) 
        {
            var result = _service.Update(bundle);
            return CreateResponse(result);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var result = _service.Delete(id);
            return CreateResponse(result);
        }

        [HttpPost("AddTourToBundle/{tourId}/{bundleId}")]
        public ActionResult AddTourToBundle(int tourId, int bundleId)
        {
            var result = _service.AddTourToBundle(tourId, bundleId);
            return CreateResponse(result);
        }

        [HttpDelete("RemoveTourFromBundle")]
        public ActionResult DeleteTourFromBundle(int tourId, int bundleId)
        {
            var result = _service.RemoveTourFromBundle(tourId, bundleId);
            return CreateResponse(result);

        }

        [HttpPut("publish/{bundleId:int}")]
        public ActionResult<BundleDto> PublishBundle(long bundleId)
        {
            var result = _service.PublishBundle(bundleId);
            return CreateResponse(result);
        }
        [HttpPut("archive/{bundleId:int}")]
        public ActionResult<BundleDto> ArchiveBundle(long bundleId)
        {
            var result = _service.ArchiveBundle(bundleId);
            return CreateResponse(result);
        }
        [HttpGet("calculate")]
        public ActionResult<double> CalculatePrice([FromQuery] long bundleId)
        {
            var result = _service.CalculatePrice(bundleId);
            return CreateResponse(result);
        }

    }
}
