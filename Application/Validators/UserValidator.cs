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
            .NotEmpty().WithMessage("Senha é obrigatória.")
            .MinimumLength(6)
                .WithMessage("Senha deve ter no mínimo 6 caracteres.")
            .MaximumLength(100)
                .WithMessage("Senha deve ter no máximo 100 caracteres.");
    }
}
