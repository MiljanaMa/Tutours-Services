namespace Explorer.Payments.API.Dtos;

public class ShoppingCartDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public List<int> OrdersId { get; set; }
    public double Price { get; set; }
}