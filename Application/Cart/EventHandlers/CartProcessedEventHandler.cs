#region

using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Event;
using MediatR;

#endregion

namespace Application.Cart.EventHandlers;

public class CartProcessedEventHandler : INotificationHandler<CartProcessedEvent>
{
    private readonly IApplicationDbContext _context;

    public CartProcessedEventHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(CartProcessedEvent notification, CancellationToken cancellationToken)
    {
        var entity = new Transaction
        {
            Detail = new Detail(),
            User = notification.Cart.User,
            PaymentMethod = notification.PaymentMethod,
            Courier = notification.Courier,
            DateTime = DateTime.Now,
        };

        foreach (var cartItem in notification.Cart.Items)
            entity.Detail.Items.Add(new DetailItem
            {
                Book = cartItem.Book,
                Amount = cartItem.Amount,
            });

        notification.Cart.Items.Clear();

        await _context.Transactions.AddAsync(entity, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }
}