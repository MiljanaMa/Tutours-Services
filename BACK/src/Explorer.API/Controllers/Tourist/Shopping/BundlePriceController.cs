using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist.Shopping
{
    [Authorize(Policy = "authorPolicy")]
    [Microsoft.AspNetCore.Mvc.Route("api/bundlePrice")]
    public class BundlePriceController : BaseApiController
    {
        private readonly IBundlePriceService _service;

        public BundlePriceController(IBundlePriceService service) 
        {
            _service = service;
        }

        [HttpGet("{bundleId:int}")]
        public ActionResult<BundlePriceDto> GetBundlePrice(long bundleId)
        {
            var result = _service.GetPriceForBundle(bundleId);
            return CreateResponse(result);
        }

        [HttpPost]
        public ActionResult<BundlePriceDto> Create([FromBody] BundlePriceDto bundlePriceDto)
        {
            var result = _service.Create(bundlePriceDto);
            return CreateResponse(result);
        }
    }
}
