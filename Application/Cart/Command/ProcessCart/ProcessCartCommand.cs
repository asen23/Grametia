#region

using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Event;
using MediatR;

#endregion

namespace Application.Cart.Command.ProcessCart;

public record ProcessCartCommand : IRequest<ValidateableResponse<Unit>>, IAuthorizeable, IValidateable
{
    public string PaymentMethod { get; set; } = default!;
    public string Courier { get; set; } = default!;
    public long UserId { get; set; } = default!;
}

public class ProcessCartCommandHandler : IRequestHandler<ProcessCartCommand, ValidateableResponse<Unit>>
{
    private readonly IApplicationDbContext _context;

    public ProcessCartCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ValidateableResponse<Unit>> Handle(ProcessCartCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await _context.Users
            .FindAsync(new object[] { request.UserId }, cancellationToken);

        if (entity == null)
            // throw new NotFoundException(nameof(Book), request.Id);
            throw new Exception("User does not exist");

        entity.Cart.AddDomainEvent(new CartProcessedEvent(entity.Cart, request.PaymentMethod, request.Courier));

        await _context.SaveChangesAsync(cancellationToken);

        return new ValidateableResponse<Unit>(Unit.Value);
    }
}