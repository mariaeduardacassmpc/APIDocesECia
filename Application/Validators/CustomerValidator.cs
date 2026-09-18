using Application.Dtos.Customer;
using Application.Helpers;
using FluentValidation;

namespace Application.Validators;

public class CreateCustomerDtoValidator : AbstractValidator<InputCustomerDto>
{
    public CreateCustomerDtoValidator()
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ApiMessages.RequiredField)
            .MaximumLength(150)
            .WithMessage(ApiMessages.Customer.NameMaxLength);

        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ApiMessages.RequiredField)
            .EmailAddress()
            .WithMessage(ApiMessages.Email.Invalid)
            .MaximumLength(100)
            .WithMessage(ApiMessages.Email.MaxLength);

        RuleFor(x => x.Phone)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ApiMessages.RequiredField)
            .Matches(@"^\+?[0-9\s\-\(\)]{8,20}$")
            .WithMessage(ApiMessages.Customer.PhoneInvalid);

        RuleFor(x => x.Address)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ApiMessages.RequiredField)
            .MaximumLength(200)
            .WithMessage(ApiMessages.Customer.AddressMaxLength);

        RuleFor(x => x.City)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ApiMessages.RequiredField)
            .MaximumLength(100)
            .WithMessage(ApiMessages.Customer.CityMaxLength);

        RuleFor(x => x.Obs)
            .MaximumLength(500)
            .WithMessage(ApiMessages.Customer.ObsMaxLength);
    }
}