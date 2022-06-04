#region

using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

#endregion

namespace Application.Books.Commands.UpdateBook;

public record UpdateBookCommand : IRequest<ValidateableResponse<Unit>>, IValidateable
{
    public long Id { get; init; } = default!;
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

public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, ValidateableResponse<Unit>>
{
    private readonly IApplicationDbContext _context;

    public UpdateBookCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ValidateableResponse<Unit>> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Books
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
            // throw new NotFoundException(nameof(TodoItem), request.Id);
            return new ValidateableResponse<Unit>(Unit.Value, "Book does not exist");

        entity.Title = request.Title == "" ? entity.Title : request.Title;
        entity.Edition = request.Edition == "" ? entity.Edition : request.Edition;
        entity.Description = request.Description == "" ? entity.Description : request.Description;
        entity.Author = request.Author == "" ? entity.Author : request.Author;
        entity.Publisher = request.Publisher == "" ? entity.Publisher : request.Publisher;
        entity.ISBN = request.ISBN == "" ? entity.ISBN : request.ISBN;
        entity.Category = request.Category == "" ? entity.Category : request.Category;
        entity.ReleaseDate = request.ReleaseDate == "" ? entity.ReleaseDate : request.ReleaseDate;
        entity.Price = request.Price == -1 ? entity.Price : request.Price;
        entity.Stock = request.Stock == -1 ? entity.Stock : request.Stock;

        await _context.SaveChangesAsync(cancellationToken);

        return new ValidateableResponse<Unit>(Unit.Value);
    }
}