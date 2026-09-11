using Application.Dtos.Product;
using ApiDoces.Services;
using ApiDoces.Services.Report;
using ApiDoces.Helpers;
using ApiDoces.Responses;
using Microsoft.AspNetCore.Mvc;

namespace ApiDoces.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController(ProductService productService, ProductReportService productReportService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateProduct(InputProductDto dto)
    {
        await productService.CreateProduct(dto);

        return Ok(ApiResponse.Success(ApiMessages.ProductCreated));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        var products = await productService.GetAllProducts();

        return Ok(ApiResponse.Success(products));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await productService.GetById(id);

        if (product == null)
            return NotFound(ApiResponse.NotFound(ApiMessages.ProductNotFound));

        return Ok(ApiResponse.Success(product));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, InputProductDto product)
    {
        var updatedProduct = await productService.UpdateProduct(id, product);

        if (updatedProduct == null)
            return NotFound(ApiResponse.NotFound(ApiMessages.ProductNotFound));

        return Ok(ApiResponse.Success(updatedProduct, ApiMessages.ProductUpdated));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var deleted = await productService.DeleteProduct(id);

        if (!deleted)
            return NotFound(ApiResponse.NotFound(ApiMessages.ProductNotFound));

        return Ok(ApiResponse.Success(ApiMessages.ProductDeleted));
    }

    [HttpGet("report")]
    public async Task<IActionResult> GetProductsReport()
    {
        var file = await productReportService.GenerateProductsReport();

        return File(
            file,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"relatorio-produtos-{DateTime.Now:yyyyMMddHHmmss}.xlsx");
    }
}