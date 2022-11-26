using CKO.PaymentGateway.BankSimulator.Events;
using CKO.PaymentGateway.Domain.Cards;
using CKO.PaymentGateway.Domain.Payments;
using MediatR;

namespace CKO.PaymentGateway.BankSimulator
{
    public class TestAcquiringBankProxy: IAcquiringBankProxy
    {
        private readonly IMediator _mediator;

        public TestAcquiringBankProxy(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task ProcessPayment(Guid id, PaymentAmount amount, CardDetails cardDetails)
        {
            var shouldSucceed = new Random().Next(0, 2) == 0;

            if (shouldSucceed)
            {
                await _mediator.Publish(new PaymentSucceeded(id));
            }
            else
            {
                await _mediator.Publish(new PaymentFailed(id));
            }
        }
    }
}