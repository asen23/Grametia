#region

using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
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
        var book = await _context.Books
            .FindAsync(new object[] { request.BookId }, cancellationToken);

        if (book == null)
            // throw new NotFoundException(nameof(TodoItem), request.Id);
            return new ValidateableResponse<Unit>(Unit.Value, "Book does not exist");

        var user = await _context.Users
            .Include(u => u.Cart)
            .SingleOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user == null)
            // throw new NotFoundException(nameof(TodoItem), request.Id);
            throw new Exception("User does not exist");

        user.Cart.Items.Add(new CartItem
        {
            Book = book,
            Amount = request.Amount,
        });

        await _context.SaveChangesAsync(cancellationToken);

        return new ValidateableResponse<Unit>(Unit.Value);
    }
}