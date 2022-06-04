#region

using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Value_Object;
using MediatR;
using Microsoft.EntityFrameworkCore;

#endregion

namespace Application.Users.Commands.LoginCommand;

public record LoginCommand : IRequest<ValidateableResponse<string>>, IValidateable
{
    public string Email { get; init; } = default!;
    public string Password { get; init; } = default!;
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, ValidateableResponse<string>>
{
    private readonly IApplicationDbContext _context;
    private readonly IUserManager _userManager;

    public LoginCommandHandler(IApplicationDbContext context, IUserManager userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<ValidateableResponse<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Where(u => u.Email == request.Email && u.Password == request.Password)
            .SingleOrDefaultAsync(cancellationToken);

        if (user == null)
            return new ValidateableResponse<string>("", "Email or Password does not match");

        _userManager.User = new CurrentUser { UserId = user.Id };

        return new ValidateableResponse<string>(user.Role);
    }
}