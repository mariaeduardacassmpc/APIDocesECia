using Application.Dtos.Sale;
using FluentValidation;

namespace ApiDoces.Validators.Sale;

public class CreateSaleValidator : AbstractValidator<InputSaleDto>
{
    public CreateSaleValidator()
    {
        RuleFor(x => x.CustomerId)
            .GreaterThan(0)
            .WithMessage("Cliente é obrigatório.");

        RuleFor(x => x.PaymentMethod)
            .NotEmpty()
            .WithMessage("Forma de pagamento é obrigatória.")
            .MaximumLength(50)
            .WithMessage("Forma de pagamento deve ter no máximo 50 caracteres.");

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("A venda deve possuir pelo menos um item.");

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
            .WithMessage("Produto é obrigatório.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("A quantidade deve ser maior que zero.");

        RuleFor(x => x.UnitPrice)
            .GreaterThan(0)
            .WithMessage("O preço unitário deve ser maior que zero.");
    }
}