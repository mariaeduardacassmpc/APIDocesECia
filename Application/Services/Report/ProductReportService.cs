using Data;
using Microsoft.EntityFrameworkCore;
using Data.Entities;

namespace ApiDoces.Services.Report;
public class ProductReportService(ApplicationDbContext context, ExcelReportService excelReportService)
{
    public async Task<byte[]> GenerateProductsReport()
    {
        var products = await context.Product.OrderBy(x => x.Name).ToListAsync();

        return excelReportService.Generate(
            "Produtos",
            products,
            new ReportColumn<Product>("ID", p => p.ProductId),
            new ReportColumn<Product>("Nome", p => p.Name),
            new ReportColumn<Product>("Descrição", p => p.Description),
            new ReportColumn<Product>("Categoria", p => p.Category),
            new ReportColumn<Product>("Imagem", p => p.Image),
            new ReportColumn<Product>("Preço de Venda", p => p.SalePrice, "R$ #,##0.00"),
            new ReportColumn<Product>("Preço de Compra", p => p.PurchasePrice, "R$ #,##0.00"),
            new ReportColumn<Product>("Estoque", p => p.Stock)
        );
    }
}