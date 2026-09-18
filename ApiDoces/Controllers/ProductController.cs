using Application.Helpers;
using ApiDoces.Responses;
using ApiDoces.Services;
using ApiDoces.Services.Report;
using Application.Dtos.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiDoces.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ProductController(ProductService productService, ProductReportService productReportService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateProduct(InputProductDto dto)
    {
        await productService.CreateProduct(dto);

        return Ok(ApiResponses.Created<object?>(null, ApiMessages.Created("Produto")));
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllProducts()
    {
        var products = await productService.GetAllProducts();

        return Ok(ApiResponses.Success(products));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await productService.GetById(id);

        return Ok(ApiResponses.Success(product));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, InputProductDto product)
    {
        var updatedProduct = await productService.UpdateProduct(id, product);

        return Ok(ApiResponses.Success(updatedProduct, ApiMessages.Updated("Produto")));
    }

    [HttpPatch("{id}/active")]
    public async Task<IActionResult> ToggleActive(int id)
    {
        var product = await productService.ToggleActive(id);

        return Ok(ApiResponses.Success(product));
    }

    [HttpGet("report")]
    public async Task<IActionResult> GetProductsReport()
    {
        var file = await productReportService.GenerateProductsReport();

        return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"relatorio-produtos-{DateTime.Now:yyyyMMddHHmmss}.xlsx");
    }
}