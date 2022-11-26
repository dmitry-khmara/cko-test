using CKO.PaymentGateway.Domain.BuildingBlocks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CKO.PaymentGateway.Application.BuildingBlocks.DomainEvents
{
    public class MediatRDomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IMediator _mediator;
        private readonly ILogger<MediatRDomainEventDispatcher> _log;
        public MediatRDomainEventDispatcher(IMediator mediator, ILogger<MediatRDomainEventDispatcher> log)
        {
            _mediator = mediator;
            _log = log;
        }

        public async Task Dispatch(IDomainEvent domainEvent)
        {

            var domainEventNotification = CreateDomainEventNotification(domainEvent);
            _log.LogDebug("Dispatching Domain Event as MediatR notification.  EventType: {eventType}", domainEvent.GetType());
            await _mediator.Publish(domainEventNotification);
        }

        private INotification CreateDomainEventNotification(IDomainEvent domainEvent)
        {
            var genericDispatcherType = typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType());
            return (INotification)Activator.CreateInstance(genericDispatcherType, domainEvent);

        }
    }
}