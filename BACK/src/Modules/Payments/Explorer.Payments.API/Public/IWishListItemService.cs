using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payments.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.API.Public;

public interface IWishListItemService
{
    Result<PagedResult<WishListItemDto>> GetPaged(int page, int pageSize);
    Result<WishListItemDto> Create(WishListItemDto wishListItem);
    Result<WishListItemDto> Update(WishListItemDto wishListItem);
    Result<PagedResult<WishListItemDto>> GetAllByUser(int page, int pageSize, int userId);
    Result Delete(int id);
}
