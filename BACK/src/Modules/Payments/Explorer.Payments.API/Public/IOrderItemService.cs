using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payments.API.Dtos;
using FluentResults;

namespace Explorer.Payments.API.Public;

public interface IOrderItemService
{
    Result<PagedResult<OrderItemDto>> GetPaged(int page, int pageSize);
    Result<OrderItemDto> Create(OrderItemDto orderItem);
    Result<OrderItemDto> Update(OrderItemDto orderItem);
    PagedResult<OrderItemDto> GetAllByUser(int page, int pageSize, int userId);
    Result Delete(int id);
}