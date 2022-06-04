#region

using Application.Common.Interfaces;
using MediatR;

#endregion

namespace Application.Common.Behaviours;

public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IAuthorizeable, IRequest<TResponse>
{
    private readonly IUserManager _userManager;

    public AuthorizationBehaviour(IUserManager userManager)
    {
        _userManager = userManager;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
        RequestHandlerDelegate<TResponse> next)
    {
        request.UserId = _userManager.User.UserId;
        return await next();
    }
}