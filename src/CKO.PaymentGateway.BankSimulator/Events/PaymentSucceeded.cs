using MediatR;

namespace CKO.PaymentGateway.BankSimulator.Events;

public class PaymentSucceeded: INotification
{
    public Guid PaymentId { get; }

    public PaymentSucceeded(Guid paymentId)
    {
        PaymentId = paymentId;
    }
}