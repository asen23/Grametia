#region

using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

#endregion

namespace Application.Users.Commands.DeleteUser;

public record DeleteUserCommand(long Id) : IRequest<ValidateableResponse<Unit>>;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, ValidateableResponse<Unit>>
{
    private readonly IApplicationDbContext _context;

    public DeleteUserCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ValidateableResponse<Unit>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Users
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
            // throw new NotFoundException(nameof(User), request.Id);
            throw new Exception("User not found");

        _context.Users.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return new ValidateableResponse<Unit>(Unit.Value);
    }
}