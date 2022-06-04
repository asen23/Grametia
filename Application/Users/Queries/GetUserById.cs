using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;

namespace Application.Users.Queries;

public record GetUserById(long Id) : IRequest<ValidateableResponse<User>>
{
}

public class GetUserByIdQueryHandler : IRequestHandler<GetUserById, ValidateableResponse<User>>
{
    private readonly IApplicationDbContext _context;

    public GetUserByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ValidateableResponse<User>> Handle(GetUserById request,
        CancellationToken cancellationToken)
    {
        var book = await _context.Users
            .FindAsync(new object[] { request.Id }, cancellationToken);

        return book == null
            ? new ValidateableResponse<User>(null!, "User does not exist")
            : new ValidateableResponse<User>(book);
    }
}