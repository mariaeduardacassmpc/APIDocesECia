using Application.Dtos.Sale;
using ApiDoces.Mappings;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ApiDoces.Services;

public class SaleService(ApplicationDbContext context, ILogger<SaleService> logger)
{
    public async Task CreateSales(InputSaleDto dto)
    {
        logger.LogInformation("Criando nova venda");

        var sale = dto.ToEntity();

        context.Sale.Add(sale);
        await context.SaveChangesAsync();

        logger.LogInformation("Venda criada com sucesso. Id: {SaleId}", sale.SaleId);
    }

    public async Task<IEnumerable<SaleListDto>> GetAllSales(string? payment, string? search, DateTime? dateStart, DateTime? dateEnd, string? sortBy)
    {
        logger.LogInformation("Buscando vendas. Payment: {Payment}, Search: {Search}, DateStart: {DateStart}, DateEnd: {DateEnd}, SortBy: {SortBy}", payment, search, dateStart, dateEnd, sortBy);

        var query = context.Sale
            .Include(s => s.Customer)
            .Include(s => s.Items)
                .ThenInclude(i => i.Product)
            .AsQueryable();

        if (!string.IsNullOrEmpty(payment) && payment != "todos")
            query = query.Where(s => s.PaymentMethod == payment);

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(s =>
                s.Customer.Name.Contains(search) ||
                s.Items.Any(i => i.Product.Name.Contains(search)));
        }

        if (dateStart.HasValue)
            query = query.Where(s => s.SaleDate >= dateStart.Value);

        if (dateEnd.HasValue)
        {
            var endDate = dateEnd.Value.Date.AddDays(1);
            query = query.Where(s => s.SaleDate < endDate);
        }

        query = sortBy switch
        {
            "antigos" => query.OrderBy(s => s.SaleDate),
            "maior" => query.OrderByDescending(s => s.TotalAmount),
            "menor" => query.OrderBy(s => s.TotalAmount),
            _ => query.OrderByDescending(s => s.SaleDate)
        };

        var sales = await query.ToListAsync();

        logger.LogInformation("Foram encontradas {Count} vendas", sales.Count);

        return sales.Select(s => s.ToListDto());
    }

    public async Task<SaleDto?> GetSaleById(int id)
    {
        logger.LogInformation("Buscando venda por Id: {SaleId}", id);

        var sale = await context.Sale.FirstOrDefaultAsync(s => s.SaleId == id);

        if (sale == null)
        {
            logger.LogWarning("Venda não encontrada. Id: {SaleId}", id);
            return null;
        }

        return sale.ToDto();
    }

    public async Task<SaleDto?> UpdateSale(int id, InputSaleDto dto)
    {
        logger.LogInformation("Atualizando venda. Id: {SaleId}", id);

        var sale = await context.Sale.FindAsync(id);

        if (sale == null)
        {
            logger.LogWarning("Venda não encontrada para atualização. Id: {SaleId}", id);
            return null;
        }

        dto.UpdateEntity(sale);
        
        context.SaleItem.RemoveRange(sale.Items);

        sale.Items = dto.Items
            .Select(item => item.ToEntity())
            .ToList();

        await context.SaveChangesAsync();

        logger.LogInformation("Venda atualizada com sucesso. Id: {SaleId}", id);

        return sale.ToDto();
    }
}