namespace CKO.PaymentGateway.Domain.Cards;

public enum InvalidCardDetailsField
{
    CardNumber,
    ExpiryDate,
    CVV
}

public class InvalidCardDetailsException: Exception
{
    public InvalidCardDetailsField Field { get; }

    public InvalidCardDetailsException(InvalidCardDetailsField field)
    {
        Field = field;
    }
}