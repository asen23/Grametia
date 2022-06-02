#region

using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

#endregion

namespace Application.Books.Commands.CreateBook;

public record CreateBookCommand : IRequest<long>
{
    public string Title { get; init; } = default!;
    public string Edition { get; init; } = default!;
    public string Description { get; init; } = default!;
    public string Author { get; init; } = default!;
    public string Publisher { get; init; } = default!;
    public string ISBN { get; init; } = default!;
    public string Category { get; init; } = default!;
    public string ReleaseDate { get; init; } = default!;
    public long Price { get; init; } = default!;
    public int Stock { get; init; } = default!;
}

public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, long>
{
    private readonly IApplicationDbContext _context;

    public CreateBookCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<long> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        var entity = new Book
        {
            Title = request.Title,
            Edition = request.Edition,
            Description = request.Description,
            Author = request.Author,
            Publisher = request.Publisher,
            ISBN = request.ISBN,
            Category = request.Category,
            ReleaseDate = request.ReleaseDate,
            Price = request.Price,
            Stock = request.Stock
        };

        _context.Books.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}