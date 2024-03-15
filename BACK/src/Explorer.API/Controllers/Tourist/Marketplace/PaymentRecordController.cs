using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public;
using Explorer.Payments.Core.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Explorer.Stakeholders.Infrastructure.Authentication;
using FluentResults;

namespace Explorer.API.Controllers.Tourist.Marketplace;

[Authorize(Policy = "touristPolicy")]
[Route("api/tourist/paymentRecords")]
public class PaymentRecordController : BaseApiController
{
    private readonly IPaymentRecordService _paymentRecordService;

    public PaymentRecordController(IPaymentRecordService paymentRecordService)
    {
        _paymentRecordService = paymentRecordService;
    }

    [HttpGet]
    [AllowAnonymous]
    public ActionResult<PagedResult<PaymentRecordDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
    {
        var result = _paymentRecordService.GetPaged(page, pageSize);
        return CreateResponse(result);
    }

    [HttpPost]
    public ActionResult<PaymentRecordDto> Create([FromBody] PaymentRecordDto paymentRecordDto)
    {
        var result = _paymentRecordService.Create(paymentRecordDto);
        return CreateResponse(result);
    }
}
