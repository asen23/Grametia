#region

using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

#endregion

namespace Application.Books.Queries;

public record GetBooks : IRequest<List<Book>>
{
}

public class GetBooksQueryHandler : RequestHandler<GetBooks, List<Book>>
{
    private readonly IApplicationDbContext _context;

    public GetBooksQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    protected override List<Book> Handle(GetBooks request)
    {
        return _context.Books.ToList();
    }
}