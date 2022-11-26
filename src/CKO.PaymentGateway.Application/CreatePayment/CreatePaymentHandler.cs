using CKO.PaymentGateway.DataAccess;
using CKO.PaymentGateway.Domain.Payments;
using MediatR;

namespace CKO.PaymentGateway.Application.CreatePayment;

public class CreatePaymentHandler : IRequestHandler<CreatePaymentCommand, Guid>
{
    private readonly PaymentGatewayContext _context;

    public CreatePaymentHandler(PaymentGatewayContext context)
    {
        _context = context;
    }
    public async Task<Guid> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = new Payment(request.MerchantId, request.Amount, request.CardDetails);

        await _context.Payments.AddAsync(payment, CancellationToken.None);

        await _context.SaveChangesAsync(CancellationToken.None);

        return payment.Id;
    }
}