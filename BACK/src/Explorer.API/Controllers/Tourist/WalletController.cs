using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public;
using Explorer.Payments.Core.UseCases;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Infrastructure.Authentication;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourist/wallet")]
    public class WalletController : BaseApiController
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService) { _walletService = walletService; }

        [HttpGet("byUser")]
        public ActionResult<PagedResult<WalletDto>> GetByUser()
        {
            var result = _walletService.GetByUser(User.PersonId());
            var resultValue = Result.Ok(result);
            return CreateResponse(resultValue);
        }
        [HttpGet("byUser/{userId:int}")]
        [AllowAnonymous]
        public ActionResult<PagedResult<WalletDto>> GetByUser(int userId)
        {
            var result = _walletService.GetByUser(userId);
            var resultValue = Result.Ok(result);
            return CreateResponse(resultValue);
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult<PagedResult<WalletDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _walletService.GetPaged(page, pageSize);
            return CreateResponse(result);
        }

        [HttpPost]
        public ActionResult<WalletDto> Create([FromBody] WalletDto wallet)
        {
            var result = _walletService.Create(wallet);
            return CreateResponse(result);
        }

        [HttpPut("addCoins/{id:int}")]
        [AllowAnonymous]
        public ActionResult<WalletDto> AddCoins([FromBody] WalletDto wallet)
        {
            var result = _walletService.AddCoins(wallet);
            return CreateResponse(result);
        }
    }
}
