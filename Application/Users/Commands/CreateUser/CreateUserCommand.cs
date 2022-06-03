#region

using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

#endregion

namespace Application.Users.Commands.CreateUser;

public record CreateUserCommand : IRequest<long>
{
    public string Username { get; init; } = default!;
    public string Address { get; init; } = default!;
    public string PhoneNumber { get; init; } = default!;
    public string Email { get; init; } = default!;
    public string Password { get; init; } = default!;
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, long>
{
    private readonly IApplicationDbContext _context;

    public CreateUserCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<long> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var entity = new User
        {
            Username = request.Username,
            Address = request.Address,
            Email = request.Email,
            Password = request.Password,
            PhoneNumber = request.PhoneNumber,
            Role = "member",
        };

        await _context.Users.AddAsync(entity, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);


        return entity.Id;
    }
}