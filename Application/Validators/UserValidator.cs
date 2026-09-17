using Application.Dtos.User;
using FluentValidation;

namespace Application.Validators;

public class UserValidator : AbstractValidator<InputUserDto>
{
    public UserValidator()
    {
        RuleFor(x => x.Email)
            .MaximumLength(100)
            .WithMessage("E-mail deve ter no máximo 100 caracteres.")
            .EmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.Email))
            .WithMessage("E-mail inválido.");

        RuleFor(x => x.Password)
            .MinimumLength(8)
            .WithMessage("Senha deve ter no mínimo 8 caracteres.")
            .MaximumLength(50)
            .WithMessage("Senha deve ter no máximo 50 caracteres.")
            .Matches("[A-Z]")
            .WithMessage("Senha deve conter pelo menos uma letra maiúscula.")
            .Matches("[a-z]")
            .WithMessage("Senha deve conter pelo menos uma letra minúscula.")
            .Matches("[0-9]")
            .WithMessage("Senha deve conter pelo menos um número.")
            .Matches("[^a-zA-Z0-9]")
            .WithMessage("Senha deve conter pelo menos um caractere especial.")
            .When(x => !string.IsNullOrWhiteSpace(x.Password));

        RuleFor(x => x)
            .Must(x =>
                !string.IsNullOrWhiteSpace(x.Email) ||
                !string.IsNullOrWhiteSpace(x.Password))
            .WithMessage("Informe pelo menos um dado para alterar.");
    }
}