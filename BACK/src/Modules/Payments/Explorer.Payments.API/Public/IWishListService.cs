using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payments.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.API.Public;

public interface IWishListService
{
    Result<PagedResult<WishListDto>> GetPaged(int page, int pageSize);
    Result<WishListDto> Create(WishListDto wishList);
    Result<WishListDto> Update(WishListDto wishList);
    WishListDto GetByUser(int userId);
    Result Delete(int id);
}
