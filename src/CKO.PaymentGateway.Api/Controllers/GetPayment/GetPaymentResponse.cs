using CKO.PaymentGateway.Domain.Payments;

namespace CKO.PaymentGateway.Api.Controllers.GetPayment;

public class GetPaymentResponse
{
    public decimal Amount { get; set; }

    public string Currency { get; set; }
    public string CardNumber { get; set; }
    public int ExpiryMonth { get; set; }
    public int ExpiryYear { get; set; }


    public string Status { get; set; }


    public GetPaymentResponse(Payment payment)
    {
        Amount = payment.Amount.Value;
        Currency = payment.Amount.Currency;
        CardNumber = payment.CardDetails.MaskedCardNumber;
        ExpiryMonth = payment.CardDetails.ExpiryMonth;
        ExpiryYear = payment.CardDetails.ExpiryYear;
        Status = payment.Status.ToString();
    }
}