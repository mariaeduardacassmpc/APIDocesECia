namespace ApiDoces.Dtos.Auth;

using Application.Dtos.Auth;
using FluentValidation;

public class AuthDtoValidator : AbstractValidator<LoginDto>
{
    public AuthDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O E-mail é obrigatório.")
            .EmailAddress().WithMessage("O E-mail informado não é válido.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("A senha é obrigatória.")
            .MinimumLength(6).WithMessage("A senha deve ter no mínimo 6 caracteres.");
    }
}