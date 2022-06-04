#region

using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;

#endregion

namespace Application.Transactions.Queries;

public record GetTransactionById : IRequest<ValidateableResponse<Transaction>>
{
    public long Id { get; set; } = default!;
}

public class GetTransactionByIdQueryHandler : IRequestHandler<GetTransactionById, ValidateableResponse<Transaction>>
{
    private readonly IApplicationDbContext _context;

    public GetTransactionByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ValidateableResponse<Transaction>> Handle(GetTransactionById request,
        CancellationToken cancellationToken)
    {
        var transaction = await _context.Transactions
            .FindAsync(new object[] { request.Id }, cancellationToken);

        return transaction == null
            ? new ValidateableResponse<Transaction>(null!, "Transaction does not exist")
            : new ValidateableResponse<Transaction>(transaction);
    }
}