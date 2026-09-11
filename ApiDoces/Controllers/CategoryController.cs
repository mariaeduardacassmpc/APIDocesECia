using ApiDoces.Services;
using ApiDoces.Dtos.Category;
using Microsoft.AspNetCore.Mvc;

namespace ApiDoces.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController(CategoryService categoryService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CategoryInputDto dto)
    {
        await categoryService.CreateCategory(dto);

        return Ok(new { message = "Category created successfully!" });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await categoryService.GetAllCategories();

        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await categoryService.GetById(id);

        if (category == null)
            return NotFound(new { message = "Category not found" });

        return Ok(category);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryDto category)
    {
        var updatedCategory = await categoryService.UpdateCategory(id, category);

        if (updatedCategory == null)
            return NotFound(new { message = "Category not found" });

        return Ok(new
        {
            message = "Category updated successfully",
            category = updatedCategory
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var deleted = await categoryService.DeleteCategory(id);

        if (!deleted)
            return NotFound(new { message = "Category not found" });

        return Ok(new
        {
            message = "Category successfully deleted!"
        });
    }
}