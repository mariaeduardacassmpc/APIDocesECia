using ApiDoces.Dtos.Product;
using ApiDoces.Services;
using ApiDoces.Services.Report;
using Microsoft.AspNetCore.Mvc;

namespace ApiDoces.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController(ProductService productService, ProductReportService productReportService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateProduct(CreateProductDto dto)
    {
        await productService.CreateProduct(dto);
        return Ok("Product created successfully!");
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        var products = await productService.GetAllProducts();
        if (products == null)
            return NotFound(new { mensagem = "Products not found" });

        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await productService.GetById(id);
        if (product == null)
            return NotFound(new { mensagem = "Product not found" });

        return Ok(product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, UpdateProductDto product)
    {
        var updatedProduct = await productService.UpdateProduct(id, product);

        if (updatedProduct == null)
            return NotFound(new { message = "Product not found" });

        return Ok(new { message = "Product updated successfully", product = updatedProduct });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var deleted = await productService.DeleteProduct(id);
        if (!deleted)
            return NotFound(new { message = "Product not found" });

        return Ok(new { message = "Product deleted successfully!" });
    }

    [HttpGet("report")]
    public async Task<IActionResult> GetProductsReport()
    {
        var file = await productReportService.GenerateProductsReport();

        return File(
            file,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"relatorio-produtos-{DateTime.Now:yyyyMMddHHmmss}.xlsx"
        );
    }
}