using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Transactions.Queries;

public record GetTransactionsByUserId : IRequest<List<Transaction>>, IAuthorizeable
{
    public long UserId { get; set; } = default!;
}

public class GetTransactionsByUserIdQueryHandler : IRequestHandler<GetTransactionsByUserId, List<Transaction>>
{
    private readonly IApplicationDbContext _context;

    public GetTransactionsByUserIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Transaction>> Handle(GetTransactionsByUserId request, CancellationToken cancellationToken)
    {
        return await _context.Transactions
            .Where(t => t.User.Id == request.UserId)
            .ToListAsync(cancellationToken);
    }
}