using Explorer.BuildingBlocks.Core.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Core.Domain.RepositoryInterfaces;

public interface IWishListRepository : ICrudRepository<WishList>
{
   WishList GetByUser(int userId);
}
