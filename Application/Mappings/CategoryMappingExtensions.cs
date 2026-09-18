using Application.Dtos.Category;
using Data.Entities;

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

    public static Category ToEntity(this CategoryInputDto dto)
    {
        return new Category
        {
            Name = dto.Name
        };
    }

    public static void UpdateEntity(this Category category, CategoryInputDto dto)
    {
        category.Name = dto.Name;
    }
}