using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public;
using Explorer.Payments.Core.Domain;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;
using FluentResults;

namespace Explorer.Payments.Core.UseCases;

public class ShoppingCartService : CrudService<ShoppingCartDto, ShoppingCart>, IShoppingCartService
{
    protected readonly IShoppingCartRepository _shoppingCartRepository;

    public ShoppingCartService(IShoppingCartRepository repository, IMapper mapper) : base(repository, mapper)
    {
        _shoppingCartRepository = repository;
    }


    public override Result<ShoppingCartDto> Update(ShoppingCartDto updatedShoppingCart)
    {
        try
        {
            var existingShoppingCart = _shoppingCartRepository.Get(updatedShoppingCart.Id);

            if (existingShoppingCart == null) return Result.Fail("Shopping cart not found.");

            existingShoppingCart.OrdersId = updatedShoppingCart.OrdersId;
            existingShoppingCart.Price = updatedShoppingCart.Price;
            _shoppingCartRepository.Update(existingShoppingCart);
            return Result.Ok(new ShoppingCartDto
            {
                UserId = existingShoppingCart.UserId,
                OrdersId = existingShoppingCart.OrdersId
            });
        }
        catch (Exception ex)
        {
            return Result.Fail($"An error occurred while updating the shopping cart: {ex.Message}");
        }
    }

    public override Result<ShoppingCartDto> Create(ShoppingCartDto entity)
    {
        var result = _shoppingCartRepository.Create(MapToDomain(entity));
        return MapToDto(result);
    }

    public ShoppingCartDto GetByUser(int userId)
    {
        var cart = _shoppingCartRepository.GetByUser(userId);
        return MapToDto(cart);
    }
}