using Application.Dtos.Category;
using FluentValidation;

public class CategoryValidator : AbstractValidator<CategoryInputDto>
{
    public CategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Nome da categoria é obrigatório.")
            .MaximumLength(100)
            .WithMessage("Nome da categoria deve ter no máximo 50 caracteres.");
    }
}