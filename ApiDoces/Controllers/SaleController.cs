using Application.Helpers;
using ApiDoces.Responses;
using ApiDoces.Services;
using Application.Dtos.Sale;
using Application.Services.Report;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiDoces.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class SalesController(SaleService salesService, SaleReportService saleReport) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateSale(InputSaleDto sale)
    {
        await salesService.CreateSales(sale);

        return Ok(ApiResponses.Created<object?>(null, ApiMessages.Created("Venda")));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSaleById(int id)
    {
        var sales = await salesService.GetSaleById(id);

        return Ok(ApiResponses.Success(sales));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllSales([FromQuery] SaleFilterDto filter)
    {
        var sales = await salesService.GetAllSales(filter);

        return Ok(ApiResponses.Success(sales));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSale(int id, InputSaleDto sale)
    {
        var updateSale = await salesService.UpdateSale(id, sale);

        return Ok(ApiResponses.Success(updateSale, ApiMessages.Updated("Venda")));
    }

    [HttpGet("report")]
    public async Task<IActionResult> GetSaleReport()
    {
        var file = await saleReport.GenerateSalesReport();

        return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"relatorio-produtos-{DateTime.Now:yyyyMMddHHmmss}.xlsx");
    }
}