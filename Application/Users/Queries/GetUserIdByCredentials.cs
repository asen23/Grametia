#region

using Application.Common.Interfaces;
using MediatR;

#endregion

namespace Application.Users.Queries;

public record GetUserIdByCredentials : IRequest<long>
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}

public class GetUserIdByCredentialsQueryHandler : RequestHandler<GetUserIdByCredentials, long>
{
    private readonly IApplicationDbContext _context;

    public GetUserIdByCredentialsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    protected override long Handle(GetUserIdByCredentials request)
    {
        return _context.Users
            .Where(u => u.Email == request.Email && u.Password == request.Password)
            .Select(x => x.Id)
            .SingleOrDefault();
    }
}