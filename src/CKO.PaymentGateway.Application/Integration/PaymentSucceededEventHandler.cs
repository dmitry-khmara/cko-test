using CKO.PaymentGateway.Application.BuildingBlocks.DomainEvents;
using CKO.PaymentGateway.BankSimulator;
using CKO.PaymentGateway.BankSimulator.Events;
using CKO.PaymentGateway.DataAccess;
using CKO.PaymentGateway.Domain.Payments.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CKO.PaymentGateway.Application.Integration;

public class PaymentSucceededEventHandler : INotificationHandler<PaymentSucceeded>
{
    private readonly PaymentGatewayContext _context;

    public PaymentSucceededEventHandler(PaymentGatewayContext context)
    {
        _context = context;
    }

    public async Task Handle(PaymentSucceeded notification, CancellationToken cancellationToken)
    {
        var payment = await _context.Payments.FirstAsync(o => o.Id == notification.PaymentId, CancellationToken.None);

        payment.HandlePaymentSucceeded();

        await _context.SaveChangesAsync(CancellationToken.None);
    }
}