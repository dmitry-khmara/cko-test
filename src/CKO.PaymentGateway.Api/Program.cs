using CKO.PaymentGateway.Application.BuildingBlocks.DomainEvents;
using CKO.PaymentGateway.Application.CreatePayment;
using CKO.PaymentGateway.BankSimulator;
using CKO.PaymentGateway.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(typeof(Program).Assembly, typeof(CreatePaymentCommand).Assembly, typeof(TestAcquiringBankProxy).Assembly);
builder.Services.AddTransient<IDomainEventDispatcher, MediatRDomainEventDispatcher>();
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(DispatchDomainEventsPipelineBehavior<,>));

PaymentGatewayContext.AddDbContext(builder.Services, builder.Configuration.GetConnectionString("Postgres"));

builder.Services.AddScoped<IAcquiringBankProxy, TestAcquiringBankProxy>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

using (var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
{
    var gatewayContext = serviceScope.ServiceProvider.GetService<PaymentGatewayContext>();

    gatewayContext.Database.EnsureCreated();
}

app.Run();
