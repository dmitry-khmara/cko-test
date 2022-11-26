using CKO.PaymentGateway.Domain.BuildingBlocks;
using Microsoft.EntityFrameworkCore;

namespace CKO.PaymentGateway.DataAccess.BuildingBlocks
{
    public abstract class DbContextBase<TContext> : DbContext, IDbContext where TContext : DbContext
    {
        protected DbContextBase(DbContextOptions<TContext> options)
            : base(options)
        {
        }

        public IEnumerable<IDomainEvent> GetDomainEvents()
        {
            var domainEventEntities = ChangeTracker.Entries<Entity>()
                .Select(po => po.Entity)
                .Where(po => po.DomainEvents.Any())
                .ToArray();

            foreach (var entity in domainEventEntities)
            {
                while (entity.DomainEvents.TryTake(out var domainEvent))
                {
                    yield return domainEvent;
                }
            }
        }

    }
}