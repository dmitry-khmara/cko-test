using CKO.PaymentGateway.Domain.Cards;
using CKO.PaymentGateway.Domain.Payments;
using MediatR;

namespace CKO.PaymentGateway.Application.CreatePaymentRequest;

public class CreatePaymentCommand: IRequest<Guid>
{
    public Guid MerchantId { get; }
    public PaymentAmount Amount { get; }

    public CardDetails CardDetails { get; }

    public CreatePaymentCommand(Guid merchantId, PaymentAmount amount, CardDetails cardDetails)
    {
        MerchantId = merchantId;
        Amount = amount;
        CardDetails = cardDetails;
    }

}