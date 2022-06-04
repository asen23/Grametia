﻿#region

using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

#endregion

namespace Application.CartItems.Queries;

public record GetCartItemByUserId : IRequest<List<CartItem>>, IAuthorizeable
{
    public long UserId { get; set; } = default!;
}

public class GetCartItemByUserIdQueryHandler : RequestHandler<GetCartItemByUserId, List<CartItem>>
{
    private readonly IApplicationDbContext _context;

    public GetCartItemByUserIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    protected override List<CartItem> Handle(GetCartItemByUserId request)
    {
        return _context.Users
            .Where(u => u.Id == request.UserId)
            .SelectMany(u => u.Cart.Items)
            .ToList();
    }
}