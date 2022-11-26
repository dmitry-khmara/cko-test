using CKO.PaymentGateway.Domain.BuildingBlocks;

namespace CKO.PaymentGateway.Application.BuildingBlocks.DomainEvents
{
    public interface IDomainEventDispatcher
    {
        Task Dispatch(IDomainEvent domainEvent);
    }
}