namespace CKO.PaymentGateway.Domain.Payments;

public class PaymentAmount
{
    public decimal Value { get; private set; }
    public string Currency { get; private set; }
    
    private PaymentAmount()
    {
    }

    public PaymentAmount(decimal value, string currency)
    {
        Value = value;
        Currency = currency;
    }
}