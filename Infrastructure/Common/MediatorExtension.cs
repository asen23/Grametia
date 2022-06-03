#region

using Domain.Common;
using Microsoft.EntityFrameworkCore;

#endregion

namespace MediatR;

public static class MediatorExtensions
{
    public static async Task DispatchDomainEvents(this IMediator mediator, DbContext context)
    {
        var entities = context.ChangeTracker
            .Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity);

        var entityList = entities.ToList();

        var domainEvents = entityList
            .SelectMany(e => e.DomainEvents)
            .ToList();

        entityList.ToList().ForEach(e => e.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
            await mediator.Publish(domainEvent);
    }
}