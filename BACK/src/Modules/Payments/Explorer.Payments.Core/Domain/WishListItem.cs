using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Core.Domain;

public class WishListItem : Entity
{
    public WishListItem()
    {
    }
    
    public WishListItem(int tourId, int userId, string tourName, string tourDescription, double tourPrice, string tourType, string tourDifficulty, double travelDistance, int tourDuration)
    {
        TourId = tourId;
        UserId = userId;
        TourName = tourName;
        TourDescription = tourDescription;
        TourPrice = tourPrice;
        TourType = tourType;
        TourDifficulty = tourDifficulty;
        TravelDistance = travelDistance;
        TourDuration = tourDuration;
    }

    public int TourId { get; set; }
    public int UserId { get; set; }
    public string TourName { get; set; }
    public string TourDescription { get; set;}
    public double TourPrice { get; set; }
    public string TourType { get; set; }
    public string TourDifficulty { get; set; }
    public double TravelDistance { get; set; }
    public int TourDuration { get; set; }

}
