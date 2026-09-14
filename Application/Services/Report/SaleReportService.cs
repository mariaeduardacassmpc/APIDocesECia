using ApiDoces.Services.Report;
using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Report;

public class SaleReportService(ApplicationDbContext context, ExcelReportService excelReportService)
{
    public async Task<byte[]> GenerateSalesReport()
    {
        var sales = await context.Sale
            .Include(x => x.Customer)
            .Include(x => x.Items)
            .ThenInclude(x => x.Product)
            .OrderByDescending(x => x.SaleDate)
            .ToListAsync();

        return excelReportService.Generate(
           "Vendas",
           sales,
           new ReportColumn<Sale>("ID", s => s.SaleId),
           new ReportColumn<Sale>("Descrição", s => s.Description),
           new ReportColumn<Sale>("Cliente", s => s.Customer.Name),
           new ReportColumn<Sale>("Forma de Pagamento", s => s.PaymentMethod),
           new ReportColumn<Sale>("Total", s => s.TotalAmount, "R$ #,##0.00"),
           new ReportColumn<Sale>("Data da Venda", s => s.SaleDate, "dd/MM/yyyy HH:mm")
       );
    }
}