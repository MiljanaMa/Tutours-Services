using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payments.API.Dtos;
using FluentResults;

namespace Explorer.Payments.API.Public;

public interface IShoppingCartService
{
    Result<ShoppingCartDto> Create(ShoppingCartDto shoppingCart);
    Result<PagedResult<ShoppingCartDto>> GetPaged(int page, int pageSize);
    Result<ShoppingCartDto> Update(ShoppingCartDto shoppingCart);
    ShoppingCartDto GetByUser(int userId);
    Result Delete(int id);
}