using Application.Dtos.Auth;
using FluentValidation;
using Application.Helpers;

namespace Application.Validators.Auth;

public class AuthDtoValidator : AbstractValidator<LoginDto>
{
    public AuthDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(ApiMessages.RequiredField)
            .EmailAddress()
            .WithMessage(ApiMessages.Email.Invalid);

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage(ApiMessages.RequiredField);
    }
}