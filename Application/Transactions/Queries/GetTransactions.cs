#region

using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

#endregion

namespace Application.Transactions.Queries;

public record GetTransactions : IRequest<List<Transaction>>
{
}

public class GetTransactionsQueryHandler : RequestHandler<GetTransactions, List<Transaction>>
{
    private readonly IApplicationDbContext _context;

    public GetTransactionsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    protected override List<Transaction> Handle(GetTransactions request)
    {
        return _context.Transactions
            .Include(t => t.Detail.Items)
            .ThenInclude(di => di.Book)
            .Include(t => t.User)
            .ToList();
    }
}