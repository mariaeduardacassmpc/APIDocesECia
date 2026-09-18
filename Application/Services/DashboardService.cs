using Application.Dtos.Dashboard;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ApiDoces.Services;

public class DashboardService(ApplicationDbContext context, ILogger<DashboardService> logger)
{
    public async Task<DashboardDto> GetDashboard()
    {
        logger.LogInformation("Buscando dados do dashboard");

        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);

        var totalProducts = await context.Product.CountAsync();
        var totalCustomers = await context.Customer.CountAsync(c => c.Active);

        var salesToday = await context.Sale
            .CountAsync(x => x.SaleDate >= today && x.SaleDate < tomorrow);

        var revenueToday = await context.Sale
            .Where(x => x.SaleDate >= today && x.SaleDate < tomorrow)
            .SumAsync(x => (decimal?)x.TotalAmount) ?? 0m;

        var salesByPaymentMethod = await context.Sale
            .GroupBy(x => x.PaymentMethod)
            .Select(x => new PaymentMethodDto
            {
                PaymentMethod = x.Key,
                Quantity = x.Count(),
                Total = x.Sum(s => s.TotalAmount)
            })
            .ToListAsync();

        var topSellingProducts = await context.SaleItem
            .GroupBy(x => x.Product.ProductId)
            .Select(x => new TopProductDto
            {
                ProductId = x.Key,
                QuantitySold = x.Sum(i => i.Quantity)
            })
            .OrderByDescending(x => x.QuantitySold)
            .Take(5)
            .ToListAsync();

        var dashboard = new DashboardDto
        {
            TotalProducts = totalProducts,
            TotalCustomers = totalCustomers,
            SalesToday = salesToday,
            RevenueToday = revenueToday,
            SalesByPaymentMethod = salesByPaymentMethod,
            TopSellingProducts = topSellingProducts
        };

        if (dashboard == null)
        {
            logger.LogWarning("Falha ao consolidar os dados do dashboard");
            throw new InvalidOperationException("Não foi possível carregar os dados do dashboard.");
        }

        logger.LogInformation("Dashboard carregado. Produtos: {Products}, Clientes: {Customers}, Vendas hoje: {SalesToday}, Receita hoje: {RevenueToday}",
            totalProducts, totalCustomers, salesToday, revenueToday);

        return dashboard;
    }
}