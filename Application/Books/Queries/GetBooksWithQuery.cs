#region

using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

#endregion

namespace Application.Books.Queries;

public record GetBooksWithQuery : IRequest<List<Book>>
{
    public string Query { get; set; } = default!;
}

public class GetBooksWithQueryQueryHandler : RequestHandler<GetBooksWithQuery, List<Book>>
{
    private readonly IApplicationDbContext _context;

    public GetBooksWithQueryQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    protected override List<Book> Handle(GetBooksWithQuery request)
    {
        return _context.Books
            .Where(x => x.Title.ToLower().Contains(request.Query.ToLower()))
            .ToList();
    }
}