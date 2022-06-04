#region

using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

#endregion

namespace Application.Users.Queries;

public record GetUsers : IRequest<List<User>>
{
}

public class GetUsersQueryHandler : RequestHandler<GetUsers, List<User>>
{
    private readonly IApplicationDbContext _context;

    public GetUsersQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    protected override List<User> Handle(GetUsers request)
    {
        return _context.Users.ToList();
    }
}