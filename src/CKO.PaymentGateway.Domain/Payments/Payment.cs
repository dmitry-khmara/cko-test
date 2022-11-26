using CKO.PaymentGateway.Domain.BuildingBlocks;
using CKO.PaymentGateway.Domain.Cards;
using CKO.PaymentGateway.Domain.Payments.Events;

namespace CKO.PaymentGateway.Domain.Payments;

public class Payment: Entity
{
    public Guid Id { get; private set; }
    public Guid MerchantId { get; private set; }
    public PaymentAmount Amount { get; private set; }

    public Guid CardDetailsId { get; private set; }
    public CardDetails CardDetails { get; private set; }

    public PaymentStatus Status { get; private set; }

    private Payment()
    {
    }

    public Payment(Guid merchantId, PaymentAmount amount, CardDetails cardDetails)
    {
        Id = Guid.NewGuid();

        MerchantId = merchantId;
        Amount = amount;
        CardDetails = cardDetails;
        CardDetailsId = cardDetails.Id;

        PublishEvent(new PaymentCreated(Id));
    }


    public void HandlePaymentSucceeded()
    {
        Status = PaymentStatus.Success;
    }

    public void HandlePaymentFailed()
    {
        Status = PaymentStatus.Rejected;
    }
}