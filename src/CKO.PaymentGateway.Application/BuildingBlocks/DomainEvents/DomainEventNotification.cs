using CKO.PaymentGateway.Domain.BuildingBlocks;
using MediatR;

namespace CKO.PaymentGateway.Application.BuildingBlocks.DomainEvents
{
    public class DomainEventNotification<TDomainEvent> : INotification where TDomainEvent : IDomainEvent
    {
        public TDomainEvent DomainEvent { get; }

        public DomainEventNotification(TDomainEvent domainEvent)
        {
            DomainEvent = domainEvent;
        }
    }
}