using System.Collections.Concurrent;

namespace CKO.PaymentGateway.Domain.BuildingBlocks;

public class Entity
{
    private readonly ConcurrentQueue<IDomainEvent> _domainEvents = new ConcurrentQueue<IDomainEvent>();

    public IProducerConsumerCollection<IDomainEvent> DomainEvents => _domainEvents;

    protected void PublishEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Enqueue(domainEvent);
    }

}