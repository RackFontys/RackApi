using System.Net;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RackApi.Chat;
using RackApi.Chat.Controllers;
using RackApi.Chat.Data;

var builder = WebApplication.CreateBuilder(args);

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddDbContext<ApiDbContext>();

// Load environment variables
builder.Configuration.AddEnvironmentVariables();

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<MessageController>();

var secretKey = builder.Configuration["JsonWebTokenStrings:DefaultJWTKey"];
var issuer = builder.Configuration["JsonWebTokenStrings:IssuerIp"];
var audience = builder.Configuration["JsonWebTokenStrings:AudienceIp"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

builder.Logging.AddConsole(); // Add console logging
builder.Logging.AddDebug(); // Add debug logging

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

// Apply Migrations
bool autoMigrate = Convert.ToBoolean(builder.Configuration["DatabaseStrings:ApplyMigrations"]);
if (autoMigrate)
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var dbContext = services.GetRequiredService<ApiDbContext>();
        dbContext.Database.Migrate();
    } 
}

app.UseCors("AllowAllOrigins");

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
var subscriber = new RabbitMQConsumer(logger, serviceScopeFactory, app.Configuration);
subscriber.ConsumeMessages();

await app.RunAsync();