using ApiDoces.Helpers;
using ApiDoces.Responses;
using ApiDoces.Services;
using Application.Dtos.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiDoces.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class CategoryController(CategoryService categoryService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CategoryInputDto dto)    
    {
        await categoryService.CreateCategory(dto);

        return Ok(ApiResponse.Success(ApiMessages.CategoryCreated));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await categoryService.GetAllCategories();

        return Ok(ApiResponse.Success(categories));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await categoryService.GetById(id);

        if (category == null)
            return NotFound(ApiResponse.NotFound(ApiMessages.CategoryNotFound));

        return Ok(ApiResponse.Success(category));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(int id, CategoryInputDto category)
    {
        var updatedCategory = await categoryService.UpdateCategory(id, category);

        if (updatedCategory == null)
            return NotFound(ApiResponse.NotFound(ApiMessages.CategoryNotFound));

        return Ok(ApiResponse.Success(updatedCategory, ApiMessages.CategoryUpdated));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var deleted = await categoryService.DeleteCategory(id);

        if (!deleted)
            return NotFound(ApiResponse.NotFound(ApiMessages.CategoryNotFound));

        return Ok(ApiResponse.Success(ApiMessages.CategoryDeleted));
    }
}