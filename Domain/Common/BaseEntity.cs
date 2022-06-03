#region

using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace Domain.Common;

public class BaseEntity
{
    private readonly List<BaseEvent> _domainEvents = new();
    public long Id { get; set; } = default!;

    [NotMapped] public IReadOnlyCollection<BaseEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void RemoveDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}