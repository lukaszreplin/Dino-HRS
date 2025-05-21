var builder = DistributedApplication.CreateBuilder(args);

var username = builder.AddParameter("username", "postgres", secret: true);
var password = builder.AddParameter("password", "postgres", secret: true);
var rabbitUsername = builder.AddParameter("rabbitUsername", "user", secret: true);
var rabbiqPassword = builder.AddParameter("rabbiqPassword", "password", secret: true);

var postgres = builder.AddPostgres("postgres", username, password).WithLifetime(ContainerLifetime.Session).WithPgAdmin();
var postgresdb = postgres.AddDatabase("reservations");

var mongo = builder.AddMongoDB("mongodb").WithLifetime(ContainerLifetime.Session).WithMongoExpress();
var mongodb = mongo.AddDatabase("hotel");

var rabbitMq = builder.AddRabbitMQ("RabbitMQConnection", rabbitUsername, rabbiqPassword).WithManagementPlugin();

var paymentsService = builder.AddProject<Projects.Dino_PaymentsService_Api>("dino-paymentsservice-api")
    .WithReference(mongodb)
    .WithReference(rabbitMq)
    .WaitFor(mongodb)
    .WaitFor(rabbitMq);

var reservationsService = builder.AddProject<Projects.Dino_ReservationsService_Api>("dino-reservationsservice-api")
    .WithReference(postgresdb)
    .WithReference(rabbitMq)
    .WaitFor(postgresdb)
    .WaitFor(rabbitMq);

//var roomService = builder.AddProject<Projects.Dino_RoomsService_Api>("dino-roomsservice-api").WithReference(mongodb);


builder.AddProject<Projects.Dino_Gateway>("dino-gateway");
//.WithReference(roomService)
//.WithReference(reservationsService);
//.WithReference(paymentsService);

builder.Build().Run();
