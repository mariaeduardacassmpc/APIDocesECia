using ApiDoces.Helpers;
using Application.Dtos.Auth;
using FluentValidation;

namespace Application.Validators;

public class PasswordResetValidator : AbstractValidator<PasswordResetDto>
{
    public PasswordResetValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(ApiMessages.RequiredField)
            .EmailAddress()
            .WithMessage(ApiMessages.Email.Invalid);

        RuleFor(x => x.Token)
            .NotEmpty()
            .WithMessage(ApiMessages.RequiredField);

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .WithMessage(ApiMessages.RequiredField)
            .MinimumLength(8)
            .WithMessage(ApiMessages.Password.MinLength)
            .MaximumLength(50)
            .WithMessage(ApiMessages.Password.MaxLength)
            .Matches("[A-Z]")
            .WithMessage(ApiMessages.Password.Uppercase)
            .Matches("[a-z]")
            .WithMessage(ApiMessages.Password.Lowercase)
            .Matches("[0-9]")
            .WithMessage(ApiMessages.Password.Number)
            .Matches("[^a-zA-Z0-9]")
            .WithMessage(ApiMessages.Password.SpecialCharacter);
    }
}