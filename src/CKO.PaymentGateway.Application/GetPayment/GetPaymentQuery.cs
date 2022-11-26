using CKO.PaymentGateway.Domain.Payments;
using MediatR;

namespace CKO.PaymentGateway.Application.GetPayment;

public class GetPaymentQuery: IRequest<Payment?>
{
    public Guid PaymentId { get; }
    public Guid MerchantId { get; }

    public GetPaymentQuery(Guid paymentId, Guid merchantId)
    {
        PaymentId = paymentId;
        MerchantId = merchantId;
    }
}