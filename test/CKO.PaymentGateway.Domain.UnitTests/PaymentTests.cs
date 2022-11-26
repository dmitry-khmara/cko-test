using System;
using CKO.PaymentGateway.Domain.Cards;
using CKO.PaymentGateway.Domain.Payments;
using CKO.PaymentGateway.Domain.Payments.Events;
using FluentAssertions;
using Xunit;

namespace CKO.PaymentGateway.Domain.UnitTests;

public class PaymentTests
{
    private CardDetails _cardDetails = new CardDetails("5105105105105100", 2, 2050, "332", DateTime.UtcNow);
    private PaymentAmount _amount = new PaymentAmount(350, "USD");
    private Guid _merchantId = Guid.NewGuid();

    [Fact]
    public void Ctor_ShouldInitializeFields_AndRaiseEvent()
    {
        var payment = new Payment(_merchantId, _amount, _cardDetails);

        payment.Id.Should().NotBe(Guid.Empty);

        payment.MerchantId.Should().Be(_merchantId);
        payment.Amount.Should().Be(_amount);
        payment.CardDetails.Should().Be(_cardDetails);
        payment.CardDetailsId.Should().Be(_cardDetails.Id);

        payment.Status.Should().Be(PaymentStatus.Pending);

        payment.DomainEvents.Should().HaveCount(1);

        var domainEvent = payment.DomainEvents.ToArray()[0];

        domainEvent.Should().BeOfType<PaymentCreated>().And.BeEquivalentTo(new PaymentCreated(payment.Id));
    }

    [Fact]
    public void HandlePaymentSucceeded_ShouldChangeStatusToSuccess()
    {
        var payment = new Payment(_merchantId, _amount, _cardDetails);

        payment.HandlePaymentSucceeded();

        payment.Status.Should().Be(PaymentStatus.Success);
    }

    [Fact]
    public void HandlePaymentFailed_ShouldChangeStatusToRejected()
    {
        var payment = new Payment(_merchantId, _amount, _cardDetails);

        payment.HandlePaymentFailed();

        payment.Status.Should().Be(PaymentStatus.Rejected);
    }
}