#region

using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

#endregion

namespace Application.Users.Commands.CreateUser;

public record CreateUserCommand : IRequest<ValidateableResponse<long>>, IValidateable
{
    public string Username { get; init; } = default!;
    public string Address { get; init; } = default!;
    public string PhoneNumber { get; init; } = default!;
    public string Email { get; init; } = default!;
    public string Password { get; init; } = default!;
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ValidateableResponse<long>>
{
    private readonly IApplicationDbContext _context;

    public CreateUserCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ValidateableResponse<long>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .SingleOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        if (user != null) return new ValidateableResponse<long>(0, "Email already registered");

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


        return new ValidateableResponse<long>(entity.Id);
    }
}