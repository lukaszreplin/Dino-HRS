var builder = DistributedApplication.CreateBuilder(args);

var username = builder.AddParameter("username", "postgres", secret: true);
var password = builder.AddParameter("password", "postgres", secret: true);
var rabbitUsername = builder.AddParameter("rabbitUsername", "user", secret: true);
var rabbiqPassword = builder.AddParameter("rabbiqPassword", "password", secret: true);

var postgres = builder.AddPostgres("postgres", username, password);
var postgresdb = postgres.AddDatabase("reservations");

//var mongodb = builder.AddMongoDB("mongodb")
//    .WithEnvironment("MONGO_INITDB_DATABASE", "hotel");

var rabbitMq = builder.AddRabbitMQ("RabbitMQConnection", rabbitUsername, rabbiqPassword).WithManagementPlugin();

//var pgAdmin = builder.AddContainer("pgadmin", "dpage/pgadmin4")
//    .WithEnvironment("PGADMIN_DEFAULT_EMAIL", "admin@hotel.com")
//    .WithEnvironment("PGADMIN_DEFAULT_PASSWORD", "admin123")
//    .WithEnvironment("PGADMIN_LISTEN_PORT", "80")
//    .WithEndpoint(targetPort: 80, port: 8080, name: "pgadmin-http");


//var paymentsService = builder.AddProject<Projects.Dino_PaymentsService_Api>("dino-paymentsservice-api")
//    .WithReference(mongodb)
//    .WithReference(rabbitMq);

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
