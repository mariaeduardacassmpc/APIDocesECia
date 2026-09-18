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

    public async Task<IEnumerable<SaleListDto>> GetAllSales(SaleFilterDto filter)
    {
        logger.LogInformation("Buscando vendas. Payment: {Payment}, Search: {Search}, DateStart: {DateStart}, DateEnd: {DateEnd}, SortBy: {SortBy}", filter.Payment, filter.Search, filter.DateStart, filter.DateEnd, filter.SortBy);

        var query = context.Sale
            .Include(s => s.Customer)
            .Include(s => s.Items)
                .ThenInclude(i => i.Product)
            .AsQueryable();

        if (!string.IsNullOrEmpty(filter.Payment) && filter.Payment != "todos")
            query = query.Where(s => s.PaymentMethod == filter.Payment);

        if (!string.IsNullOrEmpty(filter.Search))
            query = query.Where(s => s.Customer.Name.Contains(filter.Search) || s.Items.Any(i => i.Product.Name.Contains(filter.Search)));

        if (filter.DateStart.HasValue)
            query = query.Where(s => s.SaleDate >= filter.DateStart.Value);

        if (filter.DateEnd.HasValue)
        {
            var endDate = filter.DateEnd.Value.Date.AddDays(1);
            query = query.Where(s => s.SaleDate < endDate);
        }

        query = filter.SortBy switch
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

    public async Task<SaleDto> GetSaleById(int id)
    {
        logger.LogInformation("Buscando venda por Id: {SaleId}", id);

        var sale = await context.Sale.FirstOrDefaultAsync(s => s.SaleId == id);

        if (sale == null)
        {
            logger.LogWarning("Venda não encontrada. Id: {SaleId}", id);
            throw new InvalidOperationException($"Venda com Id {id} não encontrada.");
        }

        return sale.ToDto();
    }

    public async Task<SaleDto> UpdateSale(int id, InputSaleDto dto)
    {
        logger.LogInformation("Atualizando venda. Id: {SaleId}", id);

        var sale = await context.Sale.FindAsync(id);

        if (sale == null)
        {
            logger.LogWarning("Venda não encontrada para atualização. Id: {SaleId}", id);
            throw new InvalidOperationException($"Venda com Id {id} não encontrada.");
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