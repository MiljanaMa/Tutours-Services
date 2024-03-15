using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public;
using Explorer.Stakeholders.Infrastructure.Authentication;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist;

[Authorize(Policy = "userPolicy")]
[Route("api/tourist/orderItems")]
public class OrderItemController : BaseApiController
{
    private readonly IOrderItemService _orderItemService;


    public OrderItemController(IOrderItemService orderItemService)
    {
        _orderItemService = orderItemService;
    }


    [HttpGet]
    public ActionResult<PagedResult<OrderItemDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
    {
        var result = _orderItemService.GetPaged(page, pageSize);
        return CreateResponse(result);
    }


    [HttpGet("byUser")]
    public ActionResult<PagedResult<OrderItemDto>> GetAllByUser([FromQuery] int page, [FromQuery] int pageSize)
    {
        var result = _orderItemService.GetAllByUser(page, pageSize, User.PersonId());
        var resultValue = Result.Ok(result);
        return CreateResponse(resultValue);
    }


    [HttpPost]
    public ActionResult<OrderItemDto> Create([FromBody] OrderItemDto orderItem)
    {
        var result = _orderItemService.Create(orderItem);
        return CreateResponse(result);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var result = _orderItemService.Delete(id);
        return CreateResponse(result);
    }

    [HttpPut("{id:int}")]
    public ActionResult<OrderItemDto> Update([FromBody] OrderItemDto order)
    {
        var result = _orderItemService.Update(order);
        return CreateResponse(result);
    }
}