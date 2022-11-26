using CKO.PaymentGateway.DataAccess.BuildingBlocks;
using CKO.PaymentGateway.Domain.Cards;
using CKO.PaymentGateway.Domain.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CKO.PaymentGateway.DataAccess
{
    public class PaymentGatewayContext : DbContextBase<PaymentGatewayContext>
    {
        public DbSet<Payment> Payments { get; set; }

        public PaymentGatewayContext(DbContextOptions<PaymentGatewayContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var cardDetails = builder.Entity<CardDetails>();

            cardDetails.ToTable("CardDetails");
            cardDetails.Property(o => o.Id).ValueGeneratedNever();
            cardDetails.Ignore(o => o.MaskedCardNumber);

            var payment = builder.Entity<Payment>();

            payment.ToTable("Payment");
            payment.Property(o => o.Id).ValueGeneratedNever();
            payment.OwnsOne(o => o.Amount, o =>
            {
                o.Property(o => o.Value).HasColumnName("Amount");
                o.Property(o => o.Currency).HasColumnName("Currency");
            });

            payment.HasOne(o => o.CardDetails)
                .WithOne()
                .HasForeignKey<Payment>(o => o.CardDetailsId);

        }

        public static void AddDbContext(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<PaymentGatewayContext>(options =>
            {
                options.UseNpgsql(connectionString,
                    postgresOptions =>
                        postgresOptions.MigrationsAssembly(typeof(PaymentGatewayContext).Assembly.FullName));
            });

            services.AddScoped<IDbContext>(serviceProvider => serviceProvider.GetService<PaymentGatewayContext>()!);
        }
    }
}