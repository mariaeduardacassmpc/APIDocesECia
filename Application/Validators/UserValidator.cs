using Application.Helpers;
using FluentValidation;

namespace Application.Validators;

public class UserValidator : AbstractValidator<InputUserDto>
{
    public UserValidator()
    {
        RuleFor(x => x.Email)
            .MaximumLength(100)
            .WithMessage(ApiMessages.Email.MaxLength)
            .EmailAddress()
            .WithMessage(ApiMessages.Email.Invalid)
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.Password)
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
            .WithMessage(ApiMessages.Password.SpecialCharacter)
            .When(x => !string.IsNullOrWhiteSpace(x.Password));

        RuleFor(x => x)
            .Must(x =>
                !string.IsNullOrWhiteSpace(x.Email) ||
                !string.IsNullOrWhiteSpace(x.Password))
            .WithMessage(ApiMessages.RequiredField);
    }
}