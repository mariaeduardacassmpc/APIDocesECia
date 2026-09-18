using ApiDoces.Helpers;
using Application.Dtos.Expense;
using FluentValidation;

namespace Application.Validators;

public class InputExpenseDtoValidator : AbstractValidator<InputExpenseDto>
{
    public InputExpenseDtoValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage(ApiMessages.RequiredField)
            .MaximumLength(200)
            .WithMessage(ApiMessages.Expense.DescriptionMaxLength);

        RuleFor(x => x.Value)
            .GreaterThan(0)
            .WithMessage(ApiMessages.Expense.ValueGreaterThanZero);

        RuleFor(x => x.Date)
            .NotEmpty()
            .WithMessage(ApiMessages.RequiredField)
            .LessThanOrEqualTo(DateTime.Now)
            .WithMessage(ApiMessages.Expense.DateNotFuture);
    }
}