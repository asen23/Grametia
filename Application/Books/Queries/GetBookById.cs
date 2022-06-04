using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;

namespace Application.Books.Queries;

public record GetBookById(long Id) : IRequest<ValidateableResponse<Book>>
{
}

public class GetBookByIdQueryHandler : IRequestHandler<GetBookById, ValidateableResponse<Book>>
{
    private readonly IApplicationDbContext _context;

    public GetBookByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ValidateableResponse<Book>> Handle(GetBookById request,
        CancellationToken cancellationToken)
    {
        var book = await _context.Books
            .FindAsync(new object[] { request.Id }, cancellationToken);

        return book == null
            ? new ValidateableResponse<Book>(null!, "Book does not exist")
            : new ValidateableResponse<Book>(book);
    }
}