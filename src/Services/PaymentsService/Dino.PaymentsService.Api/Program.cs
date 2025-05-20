
using Dino.PaymentsService.Api.DAL;
using Dino.PaymentsService.Api.Dto;
using Dino.PaymentsService.Api.Services;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Dino.PaymentsService.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.AddServiceDefaults();

            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<PaymentRequestConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("rabbitmq", "/", h =>
                    {
                        h.Username("user");
                        h.Password("password");
                    });

                    cfg.ReceiveEndpoint("payment-requests", e =>
                    {
                        e.ConfigureConsumer<PaymentRequestConsumer>(context);
                    });
                });
            });

            var databaseSettings = builder.Configuration.GetSection("DatabaseSettings").Get<DatabaseSettings>() ?? new DatabaseSettings();
            builder.Services.AddSingleton(databaseSettings);

            builder.Services.AddScoped<IPaymentService, PaymentService>();

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.MapDefaultEndpoints();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            var paymentsApi = app.MapGroup("/payments");

            paymentsApi.MapGet("/", async (IPaymentService paymentService) =>
                Results.Ok(await paymentService.GetAll()))
                .WithName("Get all")
                .WithOpenApi();

            paymentsApi.MapGet("/{id}", async (IPaymentService paymentService, Guid id) =>
            {
                var payment = await paymentService.GetByIdAsync(id);
                return payment != null ? Results.Ok(payment) : Results.NotFound();
            })
            .WithName("Get payment by Id")
            .WithOpenApi();

            paymentsApi.MapGet("/reservation/{reservationId}", async (Guid reservationId, IPaymentService paymentService) =>
                Results.Ok(await paymentService.GetByReservationIdAsync(reservationId)))
                .WithName("GetPaymentsByReservationId")
                .WithOpenApi();

            paymentsApi.MapPost("/process", async ([FromBody] ProcessPaymentRequest request, IPaymentService paymentService) =>
            {
                var payment = await paymentService.ProcessPaymentAsync(request);

                var response = new PaymentResponse
                {
                    Id = payment.Id,
                    ReservationId = payment.ReservationId,
                    Amount = payment.Amount,
                    Currency = payment.Currency,
                    PaymentMethod = payment.PaymentMethod,
                    Status = payment.Status,
                    Date = payment.Date
                };

                return Results.Created($"/api/payments/{payment.Id}", response);
            })
            .WithName("Get payment by reservation Id")
            .WithOpenApi();

            paymentsApi.MapPut("/{id}/status", async (Guid id, PaymentStatusRequest request, IPaymentService paymentService) =>
            {
                var existingPayment = await paymentService.GetByIdAsync(id);

                if (existingPayment == null)
                    return Results.NotFound();

                var validStatuses = new[] { "Processing", "Completed", "Failed", "Refunded" };

                if (!validStatuses.Contains(request.Status))
                    return Results.BadRequest($"Invalid status. Valid values are: {string.Join(", ", validStatuses)}");

                var payment = await paymentService.UpdatePaymentStatusAsync(id, request.Status);
                return Results.Ok(payment);
            })
            .WithName("UpdatePaymentStatus")
            .WithOpenApi();

            app.Run();
        }
    }
}
