using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public;
using Explorer.Payments.Core.Domain;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Core.UseCases;

public class WishListService : CrudService<WishListDto, WishList>, IWishListService
{
    protected readonly IWishListRepository _wishListRepository;
    public WishListService(IWishListRepository repository, IMapper mapper) : base(repository, mapper)
    {
        _wishListRepository = repository; 
    }

    public WishListDto GetByUser(int userId)
    {
        var wishList = _wishListRepository.GetByUser(userId);
        return MapToDto(wishList);
    }

    public override Result<WishListDto> Create(WishListDto entity)
    {
        var result = _wishListRepository.Create(MapToDomain(entity));
        return MapToDto(result);
    }

    public override Result<WishListDto> Update(WishListDto updatedEntity)
    {
        try
        {
            var existingEntity = _wishListRepository.Get(updatedEntity.Id);
            if (existingEntity == null) return Result.Fail("Wish list not found.");

            existingEntity.WishListItemsId = updatedEntity.WishListItemsId;
            _wishListRepository.Update(existingEntity);
            return Result.Ok();
        }
        catch(Exception ex)
        {
            return Result.Fail(ex.Message);
        }
        
    }
}
