#region

using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

#endregion

namespace Application.CartItems.Commands.AddCartItem;

public record AddCartItemCommand : IRequest
{
    public long UserId { get; set; } = default!;
    public long BookId { get; set; } = default!;
    public int Amount { get; set; } = default!;
}

public class AddCartItemCommandHandler : AsyncRequestHandler<AddCartItemCommand>
{
    private readonly IApplicationDbContext _context;

    public AddCartItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    protected override async Task Handle(AddCartItemCommand request, CancellationToken cancellationToken)
    {
        var book = await _context.Books
            .FindAsync(new object[] { request.BookId }, cancellationToken);

        var user = await _context.Users
            .Include(u => u.Cart)
            .SingleOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (book == null || user == null)
            // throw new NotFoundException(nameof(TodoItem), request.Id);
            throw new Exception();

        Console.WriteLine(user.Cart.UserId);
        user.Cart.Items.Add(new CartItem
        {
            Book = book,
            Amount = request.Amount,
        });

        await _context.SaveChangesAsync(cancellationToken);
    }
}