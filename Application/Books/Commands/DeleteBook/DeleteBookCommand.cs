#region

using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

#endregion

namespace Application.Books.Commands.DeleteBook;

public record DeleteBookCommand(int Id) : IRequest;

public class DeleteBookCommandHandler : AsyncRequestHandler<DeleteBookCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteBookCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    protected override async Task Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Books
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
            // throw new NotFoundException(nameof(Book), request.Id);
            throw new Exception("tester error");

        _context.Books.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}