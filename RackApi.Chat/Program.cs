using RackApi.Chat;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// string? message;
//
// var producer = new RabbitMQProducer();
// do
// {
//     Console.WriteLine("Send a message to RabbitMQ");
//     message = Console.ReadLine();
//     producer.PublishMessage(message);
// }while(!string.Equals(message, "exit", StringComparison.CurrentCultureIgnoreCase));

app.Run();
