
using Dino.RoomsService.Api.DAL;
using Dino.RoomsService.Api.Services;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Dino.RoomsService.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.AddServiceDefaults();

            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

            var databaseSettings = builder.Configuration.GetSection("DatabaseSettings").Get<DatabaseSettings>() ?? new DatabaseSettings();
            builder.Services.AddSingleton(databaseSettings);

            builder.Services.AddScoped<IRoomService, RoomService>();

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

            var roomsApi = app.MapGroup("/rooms");

            roomsApi.MapGet("/", async (IRoomService roomService) =>
            {
                await roomService.Init();
                return Results.Ok(await roomService.GetAllAsync());
            })
                .WithName("Get all rooms")
                .WithOpenApi();

            roomsApi.MapGet("/{id}", async (IRoomService roomService, Guid id) =>
                {
                    await roomService.Init();
                    var room = await roomService.GetByIdAsync(id);
                    return room != null ? Results.Ok(room) : Results.NotFound();
                })
                .WithName("Get room by Id")
                .WithOpenApi();

            app.Run();
        }
    }
}
