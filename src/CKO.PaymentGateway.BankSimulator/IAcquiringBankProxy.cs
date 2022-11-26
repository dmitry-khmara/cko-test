using CKO.PaymentGateway.Domain.Cards;
using CKO.PaymentGateway.Domain.Payments;

namespace CKO.PaymentGateway.BankSimulator;

public interface IAcquiringBankProxy
{
    public Task ProcessPayment(Guid id, PaymentAmount amount, CardDetails cardDetails);
}