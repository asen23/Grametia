using Application.Common.Interfaces;
using Domain.Event;
using MediatR;

namespace Application.CartItems.EventHandlers;

public class CartItemModifiedEventHandler : INotificationHandler<CartItemModifiedEvent>
{
    private readonly IApplicationDbContext _context;

    public CartItemModifiedEventHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(CartItemModifiedEvent notification, CancellationToken cancellationToken)
    {
        var book = await _context.Books
            .FindAsync(new object[] { notification.BookId }, cancellationToken);

        if (book == null)
            throw new Exception("Book does not exist");

        book.Stock -= notification.ChangedAmount;

        await _context.SaveChangesAsync(cancellationToken);
    }
}