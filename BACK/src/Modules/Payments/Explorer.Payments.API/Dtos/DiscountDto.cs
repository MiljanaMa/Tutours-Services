namespace Explorer.Payments.API.Dtos;

public class DiscountDto
{
    public int Id { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public double Percentage { get; set; }
    public int UserId { get; set; }
    public ICollection<TourDiscountDto>? TourDiscounts { get; set; }
}