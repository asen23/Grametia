#region

using Application.Common.Interfaces;
using MediatR;

#endregion

namespace Application.Users.Commands.LogoutCommand;

public record LogoutCommand : IRequest
{
}

public class LogoutCommandHandler : RequestHandler<LogoutCommand>
{
    private readonly IUserManager _userManager;

    public LogoutCommandHandler(IUserManager userManager)
    {
        _userManager = userManager;
    }

    protected override void Handle(LogoutCommand request)
    {
        _userManager.User = null!;
    }
}