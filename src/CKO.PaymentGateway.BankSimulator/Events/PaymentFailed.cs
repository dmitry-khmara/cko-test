using MediatR;

namespace CKO.PaymentGateway.BankSimulator.Events;

public class PaymentFailed : INotification
{
    public Guid PaymentId { get; }

    public PaymentFailed(Guid paymentId)
    {
        PaymentId = paymentId;
    }
}