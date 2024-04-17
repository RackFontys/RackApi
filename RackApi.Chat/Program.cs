using System.Text;
using Microsoft.EntityFrameworkCore;
using RackApi.Chat;
using RackApi.Chat.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using RackApi.Chat.Controllers;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApiDbContext>(options =>
    options.UseNpgsql(connectionString));

// Add services to the container.z

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<MessageController>();

var secretKey = builder.Configuration.GetConnectionString("DefaultJWTKey");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = "http://localhost:5012",
        ValidAudience = "http://localhost:5114",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

builder.Logging.AddConsole(); // Add console logging
builder.Logging.AddDebug(); // Add debug logging

var app = builder.Build();

// Apply Migrations
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<ApiDbContext>();
    dbContext.Database.Migrate();
}

// In Configure method
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

var logger = app.Services.GetRequiredService<ILogger<RabbitMQConsumer>>();
var serviceScopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
var subscriber = new RabbitMQConsumer(logger, serviceScopeFactory);
subscriber.ConsumeMessages();

await app.RunAsync();
