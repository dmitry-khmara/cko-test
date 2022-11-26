using System;
using CKO.PaymentGateway.Domain.Cards;
using CreditCardValidator;
using FluentAssertions;
using Xunit;

namespace CKO.PaymentGateway.Domain.UnitTests
{
    public class CardDetailsTests
    {
        [Fact]
        public void Ctor_GivenAllDetailsAreCorrect_ShouldCreateCardDetails()
        {
            var validVisaCard = CreditCardFactory.RandomCardNumber(CardIssuer.Visa, 16);
            var month = 2;
            var year = 2050;
            var cvv = "332";

            var cardDetails = new CardDetails(validVisaCard, month, year, cvv, DateTime.UtcNow);

            cardDetails.Id.Should().NotBe(Guid.Empty);
            cardDetails.CardNumber.Should().Be(validVisaCard);
            cardDetails.ExpiryMonth.Should().Be(month);
            cardDetails.ExpiryYear.Should().Be(year);
            cardDetails.CVV.Should().Be(cvv);
        }

        [Fact]
        public void MaskedCardNumber_GivenAllDetailsAreCorrect_ShouldMaskFirst12Digits()
        {
            var cardDetails = new CardDetails("5105105105105100", 2, 2050, "332", DateTime.UtcNow);

            cardDetails.MaskedCardNumber.Should().Be("************5100");
        }

        [Fact]
        public void Ctor_GivenIncorrectCardNumber_ShouldThrowAnException()
        {
            var invalidCardNumber = "8923";

            var createCard = () => new CardDetails(invalidCardNumber, 2, 2050, "323", DateTime.UtcNow);

            createCard.Should()
                .Throw<InvalidCardDetailsException>()
                .And.Field.Should().Be(InvalidCardDetailsField.CardNumber);
        }

        [Theory]
        [InlineData(-1, 2050)]
        [InlineData(13, 2050)]
        [InlineData(1, 1990)]
        [InlineData(1, 2022)]
        [InlineData(1, -432)]
        public void Ctor_GivenIncorrectExpiryDateOrExpiryDateInThePast_ShouldThrowAnException(int month, int year)
        {
            var validVisaCard = CreditCardFactory.RandomCardNumber(CardIssuer.Visa, 16);

            var thirdOfFebruary2022 = new DateTime(2022, 2, 3);

            var createCard = () => new CardDetails(validVisaCard, month, year, "323",
                thirdOfFebruary2022);

            createCard.Should()
                .Throw<InvalidCardDetailsException>()
                .And.Field.Should().Be(InvalidCardDetailsField.ExpiryDate);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("1")]
        [InlineData("12")]
        [InlineData("12345")]
        [InlineData("abc")]
        public void Ctor_GivenIncorrectCVV_ShouldThrowAnException(string? cvv)
        {
            var validVisaCard = CreditCardFactory.RandomCardNumber(CardIssuer.Visa, 16);

            var createCard = () => new CardDetails(validVisaCard, 2, 2050, cvv,
                DateTime.UtcNow);

            createCard.Should()
                .Throw<InvalidCardDetailsException>()
                .And.Field.Should().Be(InvalidCardDetailsField.CVV);
        }
    }
}