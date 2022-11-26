using CKO.PaymentGateway.BankSimulator.Events;
using CKO.PaymentGateway.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CKO.PaymentGateway.Application.Integration;

public class PaymentFailedEventHandler : INotificationHandler<PaymentFailed>
{
    private readonly PaymentGatewayContext _context;

    public PaymentFailedEventHandler(PaymentGatewayContext context)
    {
        _context = context;
    }

    public async Task Handle(PaymentFailed notification, CancellationToken cancellationToken)
    {
        var payment = await _context.Payments.FirstAsync(o => o.Id == notification.PaymentId, CancellationToken.None);

        payment.HandlePaymentFailed();

        await _context.SaveChangesAsync(CancellationToken.None);
    }
}