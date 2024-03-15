using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.API.Dtos;

public class WishListItemDto
{
    public int Id { get; set; }
    public int TourId { get; set; }
    public int UserId { get; set; }
    public string TourName { get; set; }
    public string TourDescription { get; set; }
    public double TourPrice { get; set; }
    public string TourType { get; set; }
    public string TourDifficulty { get; set; }
    public double TravelDistance { get; set; }
    public int TourDuration { get; set; }
}
