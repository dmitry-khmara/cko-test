using System.ComponentModel.DataAnnotations;
using CKO.PaymentGateway.Application.CreatePayment;
using CKO.PaymentGateway.Domain.Cards;
using CKO.PaymentGateway.Domain.Payments;

namespace CKO.PaymentGateway.Api.Controllers.CreatePayment;

public class CreatePaymentRequest
{
    [Required]
    public Guid MerchantId { get; set; }

    [Required]
    public string CardNumber { get; set; }


    public int ExpiryMonth { get; set; }
    public int ExpiryYear { get; set; }

    [Required]
    public string CVV { get; set; }

    [Required]

    public decimal Amount { get; set; }

    [Required]
    public string Currency { get; set; }

    public CreatePaymentCommand CreateCommand()
    {
        var amount = new PaymentAmount(Amount, Currency);
        var cardDetails = new CardDetails(CardNumber, ExpiryMonth, ExpiryYear, CVV, DateTime.UtcNow);

        return new CreatePaymentCommand(MerchantId, amount, cardDetails);
    }
}