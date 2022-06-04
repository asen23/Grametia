#region

using FluentValidation;

#endregion

namespace Application.CartItems.Commands.UpdateCartItem;

public class UpdateCartItemCommandValidator : AbstractValidator<UpdateCartItemCommand>
{
    public UpdateCartItemCommandValidator()
    {
        RuleFor(v => v.Amount)
            .GreaterThanOrEqualTo(0);
    }
}