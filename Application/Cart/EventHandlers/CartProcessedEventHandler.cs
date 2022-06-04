#region

using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Event;
using MediatR;
using Microsoft.EntityFrameworkCore;

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

        var cart = await _context.Users
            .Include(u => u.Cart.Items)
            .Select(u => u.Cart)
            .Where(c => c.Id == notification.Cart.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if (cart == null)
            throw new Exception("Cart not found");

        foreach (var cartItem in cart.Items)
            entity.Detail.Items.Add(new DetailItem
            {
                Book = cartItem.Book,
                Amount = cartItem.Amount,
            });

        cart.Items.Clear();

        await _context.SaveChangesAsync(cancellationToken);

        await _context.Transactions.AddAsync(entity, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }
}