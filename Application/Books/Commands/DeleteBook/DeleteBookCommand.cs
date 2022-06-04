#region

using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

#endregion

namespace Application.Books.Commands.DeleteBook;

public record DeleteBookCommand(int Id) : IRequest<ValidateableResponse<Unit>>, IValidateable;

public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, ValidateableResponse<Unit>>
{
    private readonly IApplicationDbContext _context;

    public DeleteBookCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ValidateableResponse<Unit>> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Books
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
            // throw new NotFoundException(nameof(Book), request.Id);
            return new ValidateableResponse<Unit>(Unit.Value, "Book does not exist");

        _context.Books.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return new ValidateableResponse<Unit>(Unit.Value);
    }
}