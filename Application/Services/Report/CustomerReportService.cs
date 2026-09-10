using Data;
using Data.Entidades;
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
            new ReportColumn<Customer>("Name", c => c.Name),
            new ReportColumn<Customer>("Phone", c => c.Phone),
            new ReportColumn<Customer>("City", c => c.City),
            new ReportColumn<Customer>("Address", c => c.Address),
            new ReportColumn<Customer>("Email", c => c.Email),
            new ReportColumn<Customer>("Active", c => c.Active)
        );
    }
}