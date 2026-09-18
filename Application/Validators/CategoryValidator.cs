using ApiDoces.Helpers;
using Application.Dtos.Category;
using FluentValidation;

namespace Application.Validators;

public class CategoryValidator : AbstractValidator<CategoryInputDto>
{
    public CategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(ApiMessages.RequiredField)
            .MaximumLength(100)
            .WithMessage(ApiMessages.Category.NameMaxLength);
    }
}