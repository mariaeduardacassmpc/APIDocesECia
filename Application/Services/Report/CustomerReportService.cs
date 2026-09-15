using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiDoces.Services.Report;

public class CustomerReportService(ApplicationDbContext context, ExcelReportService excelReportService)
{
    public async Task<byte[]> GenerateCustomerReport()
    {
        var customers = await context.Customer.OrderBy(x => x.Name).ToListAsync();

        return excelReportService.Generate(
            "Customers",
            customers,
            new ReportColumn<Customer>("ID", c => c.CustomerId),
            new ReportColumn<Customer>("Nome", c => c.Name),
            new ReportColumn<Customer>("Telefone", c => c.Phone),
            new ReportColumn<Customer>("Cidade", c => c.City),
            new ReportColumn<Customer>("Endereço", c => c.Address),
            new ReportColumn<Customer>("Email", c => c.Email),
            new ReportColumn<Customer>("Ativo", c => c.Active)
        );
    }
}