namespace Application.Dtos.Sale;

public class InputSaleDto
{
    public string? Description { get; set; }

    public required int CustomerId { get; set; }

    public required string PaymentMethod { get; set; }

    public decimal TotalAmount { get; set; }

    public DateTime SaleDate { get; set; }

    public required List<CreateSaleItemDto> Items { get; set; }
}

public class CreateSaleItemDto
{
    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }
}