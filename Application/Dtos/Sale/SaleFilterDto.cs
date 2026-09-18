namespace Application.Dtos.Sale;

public class SaleFilterDto
{
    public string? Payment { get; set; }
    public string? Search { get; set; }
    public DateTime? DateStart { get; set; }
    public DateTime? DateEnd { get; set; }
    public string? SortBy { get; set; }
}