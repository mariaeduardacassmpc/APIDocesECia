using ApiDoces.Helpers;
using Application.Dtos.Sale;
using FluentValidation;

namespace Application.Validators;

public class CreateSaleValidator : AbstractValidator<InputSaleDto>
{
    public CreateSaleValidator()
    {
        RuleFor(x => x.CustomerId)
            .GreaterThan(0)
            .WithMessage(ApiMessages.RequiredField);

        RuleFor(x => x.PaymentMethod)
            .NotEmpty()
            .WithMessage(ApiMessages.RequiredField)
            .MaximumLength(50)
            .WithMessage(ApiMessages.Sale.PaymentMethodMaxLength);

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage(ApiMessages.Sale.ItemsRequired);

        RuleForEach(x => x.Items)
            .SetValidator(new CreateSaleItemValidator());
    }
}

public class CreateSaleItemValidator : AbstractValidator<CreateSaleItemDto>
{
    public CreateSaleItemValidator()
    {
        RuleFor(x => x.ProductId)
            .GreaterThan(0)
            .WithMessage(ApiMessages.RequiredField);

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage(ApiMessages.Sale.QuantityGreaterThanZero);

        RuleFor(x => x.UnitPrice)
            .GreaterThan(0)
            .WithMessage(ApiMessages.Sale.UnitPriceGreaterThanZero);
    }
}