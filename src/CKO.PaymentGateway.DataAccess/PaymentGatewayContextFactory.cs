using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

namespace CKO.PaymentGateway.DataAccess
{
    // Only used to construct context to run local migrations
    // See more: https://docs.microsoft.com/en-us/ef/core/cli/dbcontext-creation?tabs=dotnet-core-cli
    public class PaymentGatewayContextFactory : IDesignTimeDbContextFactory<PaymentGatewayContext>
    {
        public PaymentGatewayContext CreateDbContext(string[] args)
        {
            var services = new ServiceCollection();

            PaymentGatewayContext.AddDbContext(services, "Server=127.0.0.1;Port=5532;Database=cko;User Id=postgres;Password=postgres;");

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider.GetService<PaymentGatewayContext>()!;
        }
    }
}