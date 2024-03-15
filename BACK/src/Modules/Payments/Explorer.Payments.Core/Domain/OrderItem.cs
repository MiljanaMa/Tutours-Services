using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Payments.Core.Domain;

public class OrderItem : Entity
{
    public OrderItem()
    {
    }

    public OrderItem(int tourId, int userId, string tourName, string tourDescription, double tourPrice)
    {
        TourId = tourId;
        UserId = userId;
        TourName = tourName;
        TourDescription = tourDescription;
        TourPrice = tourPrice;
    }

    public int TourId { get; set; }
    public int UserId { get; set; }
    public string TourName { get; set; }
    public string TourDescription { get; set; }
    public double TourPrice { get; set; }
}