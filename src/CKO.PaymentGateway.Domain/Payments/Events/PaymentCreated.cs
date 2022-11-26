using CKO.PaymentGateway.Domain.BuildingBlocks;

namespace CKO.PaymentGateway.Domain.Payments.Events;

public class PaymentCreated : IDomainEvent
{
    public Guid PaymentId { get; }

    public PaymentCreated(Guid paymentId)
    {
        PaymentId = paymentId;
    }
}