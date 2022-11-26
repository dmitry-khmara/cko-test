using CKO.PaymentGateway.Application.BuildingBlocks.DomainEvents;
using CKO.PaymentGateway.BankSimulator;
using CKO.PaymentGateway.DataAccess;
using CKO.PaymentGateway.Domain.Payments.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CKO.PaymentGateway.Application.Integration;

public class PaymentCreatedEventHandler : INotificationHandler<DomainEventNotification<PaymentCreated>>
{
    private readonly PaymentGatewayContext _context;
    private readonly IAcquiringBankProxy _acquiringBank;

    public PaymentCreatedEventHandler(PaymentGatewayContext context, IAcquiringBankProxy acquiringBank)
    {
        _context = context;
        _acquiringBank = acquiringBank;
    }

    public async Task Handle(DomainEventNotification<PaymentCreated> notification, CancellationToken cancellationToken)
    {
        var payment = await _context.Payments.FirstAsync(o => o.Id == notification.DomainEvent.PaymentId, CancellationToken.None);

        await _acquiringBank.ProcessPayment(payment.Id, payment.Amount, payment.CardDetails);
    }
}