using System.Text.RegularExpressions;
using CreditCardValidator;

namespace CKO.PaymentGateway.Domain.Cards
{
    public class CardDetails
    {
        public Guid Id { get; private set; }
        public string CardNumber { get; private set; }
        public string MaskedCardNumber => new string('*', 12) + CardNumber.Substring(12, 4);

        public int ExpiryMonth { get; private set; }
        public int ExpiryYear { get; private set; }
        public string CVV { get; private set; }

        private static readonly Regex CvvRegex = new("^[0-9]{3,4}$");

        private CardDetails()
        {

        }

        public CardDetails(string cardNumber, int expiryMonth, int expiryYear, string cvv, DateTime utcNow)
        {
            Id = Guid.NewGuid();

            if (InvalidCardNumber(cardNumber))
                throw new InvalidCardDetailsException(InvalidCardDetailsField.CardNumber);

            if (InvalidExpiryDate(expiryMonth, expiryYear, utcNow))
                throw new InvalidCardDetailsException(InvalidCardDetailsField.ExpiryDate);

            if (InvalidCvv(cvv))
                throw new InvalidCardDetailsException(InvalidCardDetailsField.CVV);

            CardNumber = cardNumber;
            ExpiryMonth = expiryMonth;
            ExpiryYear = expiryYear;
            CVV = cvv;
        }

        private bool InvalidCvv(string cvv)
        {
            if (string.IsNullOrEmpty(cvv))
                return true;

            return !CvvRegex.IsMatch(cvv);
        }

        private static bool InvalidExpiryDate(int expiryMonth, int expiryYear, DateTime utcNow)
        {
            if (expiryMonth is < 1 or > 12)
                return true;

            if (expiryYear is < 2000 or > 3000)
                return true;

            var expiryDate = new DateTime(expiryYear, expiryMonth, 1);

            var expiryEndOfMonth = expiryDate.AddMonths(1).AddDays(-1);

            var currentEndOfMonth = new DateTime(utcNow.Year, utcNow.Month, 1).AddMonths(1).AddDays(-1);

            return expiryEndOfMonth < currentEndOfMonth;

        }

        private static bool InvalidCardNumber(string cardNumber)
        {
            return cardNumber?.Length != 16 || !new CreditCardDetector(cardNumber).IsValid();
        }
    }
}