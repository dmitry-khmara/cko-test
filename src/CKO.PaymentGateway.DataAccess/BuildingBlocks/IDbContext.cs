using CKO.PaymentGateway.Domain.BuildingBlocks;

namespace CKO.PaymentGateway.DataAccess.BuildingBlocks
{
    public interface IDbContext
    {
        IEnumerable<IDomainEvent> GetDomainEvents();
    }
}