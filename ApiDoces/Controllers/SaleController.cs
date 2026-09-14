using ApiDoces.Helpers;
using ApiDoces.Responses;
using ApiDoces.Services;
using Application.Dtos.Sale;
using Application.Services.Report;
using Microsoft.AspNetCore.Mvc;

namespace ApiDoces.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesController(SaleService salesService, SaleReportService saleReport) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateSale(InputSaleDto sale)
    {
        if (sale == null)
            return BadRequest(ApiResponse.BadRequest(ApiMessages.InvalidData));

        await salesService.CreateSales(sale);
        return Ok(ApiResponse.Success(ApiMessages.SaleCreated));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSaleById(int id)
    {
        var sales = await salesService.GetSaleById(id);
        if (sales == null)
            return NotFound(ApiResponse.NotFound("Venda não encontrada."));

        return Ok(ApiResponse.Success(sales));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SaleListDto>>> GetAllSales([FromQuery] string? payment, [FromQuery] string? search, [FromQuery] DateTime? dateStart, [FromQuery] DateTime? dateEnd, [FromQuery] string? sortBy)
    {
        var sales = await salesService.GetAllSales(payment, search, dateStart, dateEnd, sortBy);
        if (sales == null)
            return NotFound(ApiResponse.NotFound("Venda não encontrada."));

        return Ok(ApiResponse.Success(sales));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSale(int id, InputSaleDto sale)
    {
        var updateSale = await salesService.UpdateSale(id, sale);

        if (updateSale == null)
            return NotFound(ApiResponse.NotFound(ApiMessages.SaleNotFound));
        return Ok(ApiResponse.Success(updateSale, ApiMessages.SaleUpdated));
    }

    [HttpGet("report")]
    public async Task<IActionResult> GetSaleReport()
    {
        var file = await saleReport.GenerateSalesReport();

        return File(
            file,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"relatorio-produtos-{DateTime.Now:yyyyMMddHHmmss}.xlsx");
    }
}