using ApiDoces.Helpers;
using Application.Dtos.Product;
using FluentValidation;

namespace Application.Validators;

public class CreateProductDtoValidator : AbstractValidator<InputProductDto>
{
    public CreateProductDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(ApiMessages.RequiredField)
            .MaximumLength(150)
            .WithMessage(ApiMessages.Product.NameMaxLength);

        RuleFor(x => x.CategoryId)
            .GreaterThan(0)
            .WithMessage(ApiMessages.RequiredField);

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage(ApiMessages.Product.DescriptionMaxLength);

        RuleFor(x => x.PurchasePrice)
            .GreaterThanOrEqualTo(0)
            .WithMessage(ApiMessages.Product.PurchasePriceNotNegative);

        RuleFor(x => x.SalePrice)
            .GreaterThan(0)
            .WithMessage(ApiMessages.Product.SalePriceGreaterThanZero)
            .GreaterThanOrEqualTo(x => x.PurchasePrice)
            .WithMessage(ApiMessages.Product.SalePriceNotLessThanPurchase);

        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0)
            .WithMessage(ApiMessages.Product.StockNotNegative);

        RuleFor(x => x.Image)
            .Must(BeAValidUrlOrNull)
            .WithMessage(ApiMessages.Product.ImageUrlInvalid)
            .When(x => !string.IsNullOrEmpty(x.Image));
    }

    private bool BeAValidUrlOrNull(string? image)
    {
        return Uri.TryCreate(image, UriKind.Absolute, out _);
    }
}