using ApiDoces.Dtos.Category;
using Data.Entities;

namespace ApiDoces.Mappings;

public static class CategoryMappingExtensions
{
    public static CategoryDto ToDto(this Category category)
    {
        return new CategoryDto
        {
            Id = category.CategoryId,
            Name = category.Name
        };
    }

    public static Category ToEntity(this CreateCategoryDto dto)
    {
        return new Category
        {
            Name = dto.Name
        };
    }

    public static void UpdateFromDto(this Category category, UpdateCategoryDto dto)
    {
        category.Name = dto.Name;
    }
}