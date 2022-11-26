using CKO.PaymentGateway.DataAccess;
using CKO.PaymentGateway.Domain.Payments;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CKO.PaymentGateway.Application.GetPayment;

public class GetPaymentHandler: IRequestHandler<GetPaymentQuery, Payment?>
{
    private readonly PaymentGatewayContext _context;

    public GetPaymentHandler(PaymentGatewayContext context)
    {
        _context = context;
    }

    public async Task<Payment?> Handle(GetPaymentQuery request, CancellationToken cancellationToken)
    {
        return await _context.Payments
            .Include(o => o.CardDetails)
            .FirstOrDefaultAsync(o => o.Id == request.PaymentId && o.MerchantId == request.MerchantId, cancellationToken);
    }
}