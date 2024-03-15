using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payments.API.Public;
using Explorer.Payments.Core.Domain;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;
using Explorer.Tours.API.Internal;
using FluentResults;
using OrderItemDto = Explorer.Payments.API.Dtos.OrderItemDto;
using ShoppingCartDto = Explorer.Payments.API.Dtos.ShoppingCartDto;

namespace Explorer.Payments.Core.UseCases;

public class OrderItemService : CrudService<OrderItemDto, OrderItem>, IOrderItemService
{
    protected readonly IOrderItemRepository _orderItemRepository;

    protected readonly IShoppingCartService _shoppingCartService;
    protected readonly IInternalTourService _tourService;
    protected readonly IDiscountService _DiscountService;


    public OrderItemService(IOrderItemRepository repository, IMapper mapper,
        IShoppingCartService shoppingCartService, IInternalTourService tourService, IDiscountService DiscountService) : base(repository, mapper)
    {
        _orderItemRepository = repository;
        _shoppingCartService = shoppingCartService;
        _tourService = tourService;
        _DiscountService = DiscountService;
    }

    public override Result<OrderItemDto> Create(OrderItemDto entity)
    {
        try
        {
            var shoppingCart = _shoppingCartService.GetByUser(entity.UserId);

            if (shoppingCart == null)
            {
                CreateNewShoppingCart(entity);
            }
            else
            {
                if (IsTourAlreadyInCart(shoppingCart, entity.TourId))
                {
                    return Result.Fail(FailureCode.Conflict)
                                 .WithError("This tour is already in the shopping cart!");
                }

                AddItemToShoppingCart(shoppingCart, entity);
                _shoppingCartService.Update(shoppingCart);
            }

            var result = CrudRepository.Create(MapToDomain(entity));
            return MapToDto(result);
        }
        catch (ArgumentException e)
        {
            return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
        }
    }

    private void CreateNewShoppingCart(OrderItemDto entity)
    {
        var price = _tourService.Get(entity.TourId).Value.Price;

        var newShoppingCart = new ShoppingCartDto
        {
            Id = 1,
            UserId = entity.UserId,
            Price = price,
            OrdersId = new List<int> { entity.Id }
        };
        _shoppingCartService.Create(newShoppingCart);
    }

    private bool IsTourAlreadyInCart(ShoppingCartDto shoppingCart, int tourId)
    {
        return shoppingCart.OrdersId.Any(orderId =>
               _orderItemRepository.Get(orderId).TourId == tourId);
    }

    private void AddItemToShoppingCart(ShoppingCartDto shoppingCart, OrderItemDto entity)
    {
        shoppingCart.OrdersId.Add(entity.Id);
        var originalPrice = _tourService.Get(entity.TourId).Value.Price;
        var discount = _DiscountService.GetDiscountForTour(entity.TourId).Value;
        var discountedPrice = originalPrice - (originalPrice * (discount / 100));
        shoppingCart.Price += discountedPrice;
    }



    public PagedResult<OrderItemDto> GetAllByUser(int page, int pageSize, int currentUserId)
    {
        var shoppingCart = _shoppingCartService.GetByUser(currentUserId);
        var result = GetPaged(page, pageSize);

        var filteredItems = result
            .ValueOrDefault
            .Results
            .Where(order => shoppingCart != null &&
                            shoppingCart.OrdersId.Contains(order.Id) &&
                            order.UserId == currentUserId)
            .ToList();

        return new PagedResult<OrderItemDto>(filteredItems, filteredItems.Count);
    }


    public override Result Delete(int id)
    {
        try
        {
            var orderItem = _orderItemRepository.Get(id);
            var shoppingCart = _shoppingCartService.GetByUser(orderItem.UserId);

            RemoveOrderFromShoppingCart(id, shoppingCart);

            if (shoppingCart.OrdersId.Count == 0)
            {
                _shoppingCartService.Delete(shoppingCart.Id);
            }

            _orderItemRepository.Delete(id);

            return Result.Ok();
        }
        catch (KeyNotFoundException e)
        {
            return Result.Fail(FailureCode.NotFound).WithError(e.Message);
        }
    }

    private void RemoveOrderFromShoppingCart(int orderId, ShoppingCartDto shoppingCart)
    {
        for (var i = shoppingCart.OrdersId.Count - 1; i >= 0; i--)
        {
            if (orderId == shoppingCart.OrdersId[i])
            {
                shoppingCart.OrdersId.RemoveAt(i);
                _shoppingCartService.Update(shoppingCart);
            }
        }
    }

}