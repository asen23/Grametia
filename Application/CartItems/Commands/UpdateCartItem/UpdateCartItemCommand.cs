#region

using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

#endregion

namespace Application.CartItems.Commands.UpdateCartItem;

public record UpdateCartItemCommand : IRequest<ValidateableResponse<Unit>>, IAuthorizeable, IValidateable
{
    public long BookId { get; set; } = default!;
    public int Amount { get; set; } = default!;
    public long UserId { get; set; } = default!;
}

public class UpdateCartItemCommandHandler : IRequestHandler<UpdateCartItemCommand, ValidateableResponse<Unit>>
{
    private readonly IApplicationDbContext _context;

    public UpdateCartItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ValidateableResponse<Unit>> Handle(UpdateCartItemCommand request,
        CancellationToken cancellationToken)
    {
        var cart = await _context.Users
            .Where(u => u.Id == request.UserId)
            .Select(u => u.Cart)
            .SingleOrDefaultAsync(cancellationToken);

        if (cart == null)
            // throw new NotFoundException(nameof(TodoItem), request.Id);
            throw new Exception("Cart does not exist");

        var cartItem = cart.Items
            .SingleOrDefault(ci => ci.Book.Id == request.BookId);

        if (cartItem == null)
            // throw new NotFoundException(nameof(TodoItem), request.Id);
            return new ValidateableResponse<Unit>(Unit.Value, "Cart Item does not exist");

        if (request.Amount == 0)
            cart.Items.Remove(cartItem);
        else
            cartItem.Amount = request.Amount;

        await _context.SaveChangesAsync(cancellationToken);

        return new ValidateableResponse<Unit>(Unit.Value);
    }
}