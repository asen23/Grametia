#region

using FluentValidation;

#endregion

namespace Application.CartItems.Commands.AddCartItem;

public class AddCartItemCommandValidator : AbstractValidator<AddCartItemCommand>
{
    public AddCartItemCommandValidator()
    {
        RuleFor(v => v.Amount)
            .GreaterThan(0);
    }
}