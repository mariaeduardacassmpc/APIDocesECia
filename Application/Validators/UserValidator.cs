using Application.Dtos.User;
using FluentValidation;


namespace Application.Validators;

public class UserValidator : AbstractValidator<InputUserDto>
{
    public UserValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("E-mail é obrigatório.")
            .MaximumLength(100)
            .WithMessage("E-mai deve ter no máximo 50 caracteres.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Senha é obrigatória.")
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
            .WithMessage("Senha deve conter pelo menos um caractere especial.");
    }
}
