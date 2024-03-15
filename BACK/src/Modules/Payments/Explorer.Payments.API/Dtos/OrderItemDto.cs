namespace Explorer.Payments.API.Dtos;

public class OrderItemDto
{
    public int Id { get; set; }
    public int TourId { get; set; }
    public int UserId { get; set; }
    public string TourName { get; set; }
    public string TourDescription { get; set; }
    public double TourPrice { get; set; }
}