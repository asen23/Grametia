#region

using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

#endregion

namespace Application.CartItems.Commands.UpdateCartItem;

public record UpdateCartItemCommand : IRequest
{
    public long UserId { get; set; } = default!;
    public long BookId { get; set; } = default!;
    public int Amount { get; set; } = default!;
}

public class UpdateCartItemCommandHandler : AsyncRequestHandler<UpdateCartItemCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateCartItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    protected override async Task Handle(UpdateCartItemCommand request, CancellationToken cancellationToken)
    {
        var cartItem = await _context.Users
            .Where(u => u.Id == request.UserId)
            .SelectMany(u => u.Cart.Items)
            .Where(ci => ci.Book.Id == request.BookId)
            .SingleOrDefaultAsync(cancellationToken);

        if (cartItem == null)
            // throw new NotFoundException(nameof(TodoItem), request.Id);
            throw new Exception();

        await _context.SaveChangesAsync(cancellationToken);
    }
}