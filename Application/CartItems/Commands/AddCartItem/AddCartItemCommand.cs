#region

using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Domain.Event;
using MediatR;
using Microsoft.EntityFrameworkCore;

#endregion

namespace Application.CartItems.Commands.AddCartItem;

public record AddCartItemCommand : IRequest<ValidateableResponse<Unit>>, IAuthorizeable, IValidateable
{
    public long BookId { get; set; } = default!;
    public int Amount { get; set; } = default!;
    public long UserId { get; set; } = default!;
}

public class AddCartItemCommandHandler : IRequestHandler<AddCartItemCommand, ValidateableResponse<Unit>>
{
    private readonly IApplicationDbContext _context;

    public AddCartItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ValidateableResponse<Unit>> Handle(AddCartItemCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(u => u.Cart.Items)
            .SingleOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user == null)
            // throw new NotFoundException(nameof(TodoItem), request.Id);
            throw new Exception("User does not exist");

        var entity = user.Cart.Items
            .SingleOrDefault(ci => ci.Book.Id == request.BookId);

        if (entity != null)
            return new ValidateableResponse<Unit>(Unit.Value, "Book already added to cart");
        
        var book = await _context.Books
            .FindAsync(new object[] { request.BookId }, cancellationToken);

        if (book == null)
            // throw new NotFoundException(nameof(TodoItem), request.Id);
            return new ValidateableResponse<Unit>(Unit.Value, "Book does not exist");

        if (book.Stock < request.Amount)
            return new ValidateableResponse<Unit>(Unit.Value, "Book does not have enough stock");

        var cartItem = new CartItem
        {
            Book = book,
            Amount = request.Amount,
        };

        cartItem.AddDomainEvent(new CartItemModifiedEvent(request.Amount, request.BookId));

        user.Cart.Items.Add(cartItem);

        await _context.SaveChangesAsync(cancellationToken);

        return new ValidateableResponse<Unit>(Unit.Value);
    }
}