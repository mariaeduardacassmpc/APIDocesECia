using FluentValidation;
using ApiDoces.Dtos.Customer;

namespace ApiDoces.Dtos.Customer;

public class CreateCustomerDtoValidator : AbstractValidator<CreateCustomerDto>
{
    public CreateCustomerDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .MaximumLength(150).WithMessage("O nome deve ter no máximo 150 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O e-mail é obrigatório.")
            .EmailAddress().WithMessage("O e-mail informado não é válido.")
            .MaximumLength(150).WithMessage("O e-mail deve ter no máximo 150 caracteres.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("O telefone é obrigatório.")
            .Matches(@"^\+?[0-9\s\-\(\)]{8,20}$").WithMessage("O telefone informado não é válido.");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("O endereço é obrigatório.")
            .MaximumLength(200).WithMessage("O endereço deve ter no máximo 200 caracteres.");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("A cidade é obrigatória.")
            .MaximumLength(100).WithMessage("A cidade deve ter no máximo 100 caracteres.");

        RuleFor(x => x.Obs)
            .MaximumLength(500).WithMessage("As observações devem ter no máximo 500 caracteres.");
    }
}