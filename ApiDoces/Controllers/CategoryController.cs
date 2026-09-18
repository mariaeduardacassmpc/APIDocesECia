using Application.Helpers;
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

        return Ok(ApiResponses.Success(ApiMessages.Created("Categoria")));
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var categories = await categoryService.GetAllCategories();

        return Ok(ApiResponses.Success(categories));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await categoryService.GetById(id);

        return Ok(ApiResponses.Success(category));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(int id, CategoryInputDto category)
    {
        var updatedCategory = await categoryService.UpdateCategory(id, category);

        return Ok(ApiResponses.Success(updatedCategory, ApiMessages.Updated("Categoria")));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var deleted = await categoryService.DeleteCategory(id);

        return Ok(ApiResponses.Success(ApiMessages.Deleted("Categoria")));
    }
}