using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers
{
    [Authorize(Policy = "userPolicy")]
    [Route("api/xp")]
    public class XPController : BaseApiController
    {
        private readonly IXPService _xpService;

        public XPController(IXPService xpService)
        {
            _xpService = xpService;
        }

        [HttpGet("fight/{clubFightId:int}")]
        public ActionResult<ClubFightXPInfoDto> GetClubFightXPInfo([FromRoute] int clubFightId)
        {
            var result = _xpService.GetClubFightXPInfo(clubFightId);
            return CreateResponse(result);
        }

        [HttpGet("fight/update")]
        public ActionResult<Result> SneakyUpdateFights()
        {
            _xpService.UpdateFights(false);
            return CreateResponse(Result.Ok());
        }
    }
}
