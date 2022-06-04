﻿#region

using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;

#endregion

namespace Application.Books.Commands.CreateBook;

public record CreateBookCommand : IRequest<ValidateableResponse<long>>, IValidateable
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

public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, ValidateableResponse<long>>
{
    private readonly IApplicationDbContext _context;

    public CreateBookCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ValidateableResponse<long>> Handle(CreateBookCommand request, CancellationToken cancellationToken)
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
            Stock = request.Stock,
        };

        await _context.Books.AddAsync(entity, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return new ValidateableResponse<long>(entity.Id);
    }
}