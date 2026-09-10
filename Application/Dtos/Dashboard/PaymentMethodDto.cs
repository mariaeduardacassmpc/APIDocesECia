namespace ApiDoces.Dtos.Dashboard;

public class PaymentMethodDto
{
    public string PaymentMethod { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Total { get; set; }
}