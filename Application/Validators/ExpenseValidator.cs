using Application.Dtos.Expense;
using FluentValidation;

public class InputExpenseDtoValidator : AbstractValidator<InputExpenseDto>
{
    public InputExpenseDtoValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("A descrição é obrigatória.")
            .MaximumLength(200).WithMessage("A descrição deve ter no máximo 200 caracteres.");

        RuleFor(x => x.Value)
            .GreaterThan(0).WithMessage("O valor deve ser maior que zero.");

        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("A data é obrigatória.")
            .LessThanOrEqualTo(DateTime.Now).WithMessage("A data não pode ser futura.");
    }
}