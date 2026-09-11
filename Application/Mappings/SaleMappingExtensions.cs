using Application.Dtos.Sale;
using Data.Entities;

namespace ApiDoces.Mappings;

public static class SaleMappingExtensions
{
    public static SaleDto ToDto(this Sale sale)
    {
        return new SaleDto
        {
            Id = sale.SaleId,
            Description = sale.Description,
            CustomerId = sale.CustomerId,
            PaymentMethod = sale.PaymentMethod,
            TotalAmount = sale.TotalAmount,
            SaleDate = sale.SaleDate,

            Items = sale.Items
                .Select(item => item.ToDto())
                .ToList()
        };
    }

    public static SaleItemDto ToDto(this SaleItem item)
    {
        return new SaleItemDto
        {
            Id = item.SaleItemId,
            ProductId = item.ProductId,
            Quantity = item.Quantity,
            UnitPrice = item.UnitPrice,
            Subtotal = item.Subtotal
        };
    }

    public static Sale ToEntity(this InputSaleDto dto)
    {
        return new Sale
        {
            Description = dto.Description,
            CustomerId = dto.CustomerId,
            PaymentMethod = dto.PaymentMethod,
            SaleDate = dto.SaleDate,
            TotalAmount = dto.Items.Sum(i => i.Quantity * i.UnitPrice),

            Items = dto.Items
                .Select(i => i.ToEntity())
                .ToList()
        };
    }

    public static SaleItem ToEntity(this CreateSaleItemDto dto)
    {
        return new SaleItem
        {
            ProductId = dto.ProductId,
            Quantity = dto.Quantity,
            UnitPrice = dto.UnitPrice
        };
    }

    public static SaleListDto ToListDto(this Sale sale)
    {
        return new SaleListDto
        {
            Id = sale.SaleId,
            Description = sale.Description,
            CustomerId = sale.CustomerId,
            CustomerName = sale.Customer.Name,
            PaymentMethod = sale.PaymentMethod,
            TotalAmount = sale.TotalAmount,
            SaleDate = sale.SaleDate,

            Items = sale.Items.Select(item => new SaleListItemDto
            {
                ProductId = item.ProductId,
                ProductName = item.Product.Name,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                Subtotal = item.Subtotal
            }).ToList()
        };
    }
}