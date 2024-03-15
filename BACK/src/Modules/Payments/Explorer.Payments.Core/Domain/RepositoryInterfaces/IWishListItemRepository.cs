using Explorer.BuildingBlocks.Core.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Core.Domain.RepositoryInterfaces;

public interface IWishListItemRepository : ICrudRepository<WishListItem>
{
    public PagedResult<WishListItem> GetByUser(int page, int pageSize, int userId);
    public void RemoveRange(List<int> wishListItemsIds);
}
