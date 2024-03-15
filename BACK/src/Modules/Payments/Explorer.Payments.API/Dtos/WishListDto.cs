using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.API.Dtos;

public class WishListDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public List<int> WishListItemsId { get; set; }
}
