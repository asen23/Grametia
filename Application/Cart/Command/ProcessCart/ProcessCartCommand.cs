#region

using Application.Common.Interfaces;
using Domain.Event;
using MediatR;

#endregion

namespace Application.Cart.Command.ProcessCart;

public record ProcessCartCommand : IRequest
{
    public long UserId { get; set; } = default!;
    public string PaymentMethod { get; set; } = default!;
    public string Courier { get; set; } = default!;
}

public class ProcessCartCommandHandler : AsyncRequestHandler<ProcessCartCommand>
{
    private readonly IApplicationDbContext _context;

    public ProcessCartCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    protected override async Task Handle(ProcessCartCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Users
            .FindAsync(new object[] { request.UserId }, cancellationToken);

        if (entity == null)
            // throw new NotFoundException(nameof(Book), request.Id);
            throw new Exception("tester error");

        entity.Cart.AddDomainEvent(new CartProcessedEvent(entity.Cart, request.PaymentMethod, request.Courier));

        await _context.SaveChangesAsync(cancellationToken);
    }
}