#region

using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

#endregion

namespace Application.Transactions.Queries;

public record GetTransactionById(long Id) : IRequest<ValidateableResponse<Transaction>>, IAuthorizeable
{
    public long UserId { get; set; }
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
            .Include(t => t.User)
            .Include(t => t.Detail.Items)
            .SingleOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (transaction == null)
            return new ValidateableResponse<Transaction>(null!, "Transaction does not exist");

        var user = await _context.Users
            .FindAsync(new object[] { request.UserId }, cancellationToken);

        if (user == null)
            throw new Exception("User does not exist");

        if (transaction.User.Id != request.UserId && user.Role != "admin")
            return new ValidateableResponse<Transaction>(null!, "You cannot access transaction from other account");
        
        return new ValidateableResponse<Transaction>(transaction);
    }
}