using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public;
using Explorer.Payments.Core.Domain;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;
using Explorer.Tours.API.Internal;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Core.UseCases;

public class WishListItemService : CrudService<WishListItemDto, WishListItem>, IWishListItemService
{
    protected readonly IWishListItemRepository _wishListItemRepository;
    protected readonly IWishListService _wishListService;
    protected readonly IInternalTourService _internalTourService;

    public WishListItemService(IWishListItemRepository repository, IMapper mapper, IWishListService wishListService, IInternalTourService internalTourService) : base(repository, mapper)
    {
        _wishListItemRepository = repository;
        _internalTourService = internalTourService;
        _wishListService = wishListService;
    }

    public override Result<WishListItemDto> Create(WishListItemDto entity)
    {
        try
        {
            var wishList = _wishListService.GetByUser(entity.UserId);
            if (wishList == null)
            {
                var newCreatedWishListItem = _wishListItemRepository.Create(MapToDomain(entity));
                var newItemDto = MapToDto(newCreatedWishListItem);
                CreateNewWishList(newItemDto);
                wishList = _wishListService.GetByUser((int)newItemDto.UserId);
                wishList.WishListItemsId.Add((int)newItemDto.Id);
                //AddItemToWishList(wishList, MapToDto(newCreatedWishListItem));
                _wishListService.Update(wishList);

                // var newresult = CrudRepository.Create(MapToDomain(entity));
                return MapToDto(newCreatedWishListItem);
            }

            if (IsTourAlreadyInWishList(wishList, entity.TourId))
            {
                return Result.Fail(FailureCode.Conflict).WithError("This tour is already in the whish list.");
            }

            var createdWishListItem = _wishListItemRepository.Create(MapToDomain(entity));
            var itemDto = MapToDto(createdWishListItem);
            wishList = _wishListService.GetByUser((int)itemDto.UserId);
            wishList.WishListItemsId.Add((int)itemDto.Id);
            //AddItemToWishList(wishList, MapToDto(createdWishListItem));
            _wishListService.Update(wishList);
            
            //var result = CrudRepository.Create(MapToDomain(entity));
            return MapToDto(createdWishListItem);
        }
        catch(ArgumentException e)
        {
            return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
        }
    }

    
  
    public Result<PagedResult<WishListItemDto>> GetAllByUser(int page, int pageSize, int userId)
    {
        var pagedResult = _wishListItemRepository.GetByUser(page, pageSize, userId); // GetPaged(1, 1);
        return MapToDto(pagedResult);

    }

  


    private void CreateNewWishList(WishListItemDto entity)
    {
        var newWishList = new WishListDto
        {
            UserId = entity.UserId,
            WishListItemsId = new List<int> { entity.Id }
        };
        var wishList = _wishListService.Create(newWishList);
        //return wishList;
    }

    private bool IsTourAlreadyInWishList(WishListDto wishList, int tourId)
    {
        return wishList.WishListItemsId.Any(itemId => _wishListItemRepository.Get(itemId).TourId == tourId);
    }

    private void AddItemToWishList(WishListDto wishList, WishListItemDto entity)
    {
        wishList.WishListItemsId.Add(entity.Id);
    }

    private void RemoveItemFromWishList(int tourId, WishListDto wishList)
    {
        for(var i = wishList.WishListItemsId.Count - 1; i >= 0; i--)
        {
            if(tourId == wishList.WishListItemsId[i])
            {
                wishList.WishListItemsId.RemoveAt(i);
                _wishListService.Update(wishList);
            }
        }
    }

    public override Result Delete(int id)
    {
        try
        {
            var item = _wishListItemRepository.Get(id);
            var wishList = _wishListService.GetByUser(item.UserId);
            RemoveItemFromWishList(id, wishList);
            _wishListItemRepository.Delete(id);
            return Result.Ok();
        }
        catch(KeyNotFoundException e)
        {
            return Result.Fail(FailureCode.NotFound).WithError(e.Message);
        }
    }

 

}
