namespace Application.Dtos.Dashboard;

public class DashboardDto
{
    public int TotalProducts { get; set; }
    public int TotalCustomers { get; set; }
    public int SalesToday { get; set; }
    public decimal RevenueToday { get; set; }
    public List<PaymentMethodDto> SalesByPaymentMethod { get; set; } = [];
    public List<TopProductDto> TopSellingProducts { get; set; } = [];
}