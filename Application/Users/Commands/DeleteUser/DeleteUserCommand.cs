#region

using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

#endregion

namespace Application.Users.Commands.DeleteUser;

public record DeleteUserCommand(int Id) : IRequest;

public class DeleteUserCommandHandler : AsyncRequestHandler<DeleteUserCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteUserCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    protected override async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Users
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
            // throw new NotFoundException(nameof(User), request.Id);
            throw new Exception();

        _context.Users.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}