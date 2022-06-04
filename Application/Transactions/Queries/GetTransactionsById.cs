using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Transactions.Queries;

public record GetTransactionsById : IRequest<List<Transaction>>
{
    public long Id { get; set; } = default!;
}

public class GetTransactionsByIdQueryHandler : IRequestHandler<GetTransactionsById, List<Transaction>>
{
    private readonly IApplicationDbContext _context;

    public GetTransactionsByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Transaction>> Handle(GetTransactionsById request, CancellationToken cancellationToken)
    {
        return await _context.Transactions
            .Where(t => t.Id == request.Id)
            .ToListAsync(cancellationToken);
    }
}