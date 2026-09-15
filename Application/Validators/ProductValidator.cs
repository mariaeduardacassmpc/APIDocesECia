namespace Application.Dtos.Product;

using FluentValidation;

public class CreateProductDtoValidator : AbstractValidator<InputProductDto>
{
    public CreateProductDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .MaximumLength(150).WithMessage("O nome deve ter no máximo 150 caracteres.");

        RuleFor(x => x.CategoryId)
             .GreaterThan(0)
             .WithMessage("A categoria é obrigatória.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("A descrição deve ter no máximo 500 caracteres.");

        RuleFor(x => x.PurchasePrice)
            .GreaterThanOrEqualTo(0).WithMessage("O preço de compra não pode ser negativo.");

        RuleFor(x => x.SalePrice)
            .GreaterThan(0).WithMessage("O preço de venda deve ser maior que zero.")
            .GreaterThanOrEqualTo(x => x.PurchasePrice).WithMessage("O preço de venda não pode ser menor que o preço de compra.");

        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0).WithMessage("O estoque não pode ser negativo.");

        RuleFor(x => x.Image)
            .Must(BeAValidUrlOrNull).WithMessage("A URL da imagem informada não é válida.")
            .When(x => !string.IsNullOrEmpty(x.Image));
    }

    private bool BeAValidUrlOrNull(string? image)
    {
        return Uri.TryCreate(image, UriKind.Absolute, out _);
    }
}