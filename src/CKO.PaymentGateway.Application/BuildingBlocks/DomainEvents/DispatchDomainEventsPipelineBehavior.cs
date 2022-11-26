using CKO.PaymentGateway.DataAccess.BuildingBlocks;
using MediatR;

namespace CKO.PaymentGateway.Application.BuildingBlocks.DomainEvents
{
    public class DispatchDomainEventsPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IDbContext _context;
        private readonly IDomainEventDispatcher _dispatcher;

        public DispatchDomainEventsPipelineBehavior(IDbContext context, IDomainEventDispatcher dispatcher)
        {
            _context = context;
            _dispatcher = dispatcher;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            TResponse response = await next();

            var events = _context.GetDomainEvents().ToArray();

            while (events.Any())
            {
                foreach (var @event in events)
                {
                    await _dispatcher.Dispatch(@event);
                }

                events = _context.GetDomainEvents().ToArray();
            }

            return response;
        }
    }
}