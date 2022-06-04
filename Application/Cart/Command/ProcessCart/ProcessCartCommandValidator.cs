#region

using FluentValidation;

#endregion

namespace Application.Cart.Command.ProcessCart;

public class ProcessCartCommandValidator : AbstractValidator<ProcessCartCommand>
{
    public ProcessCartCommandValidator()
    {
        RuleFor(v => v.PaymentMethod)
            .NotEmpty();
        RuleFor(v => v.Courier)
            .NotEmpty();
    }
}