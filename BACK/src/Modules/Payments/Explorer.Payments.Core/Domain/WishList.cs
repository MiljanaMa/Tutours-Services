using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Core.Domain;

public class WishList : Entity
{
    public WishList() { }

    public WishList(int userId, List<int> wishListItemsIs)
    {
        UserId = userId;
        WishListItemsId = wishListItemsIs;
    }

    public int UserId { get; set; }
    public List<int> WishListItemsId {  get; set; }
}
