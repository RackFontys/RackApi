using System.Net;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.AddJsonFile("Ocelot.json", false, true);
builder.Services.AddOcelot(builder.Configuration);

builder.Logging.AddConsole();

var secretKey = builder.Configuration["JsonWebTokenStrings:DefaultJWTKey"];
var issuer = builder.Configuration["JsonWebTokenStrings:IssuerIp"];
var audience = builder.Configuration["JsonWebTokenStrings:AudienceIp"];
var ocelotUser = builder.Configuration["Ocelot:UserService"];
var ocelotMessage = builder.Configuration["Ocelot:UserService"];
var ocelotAddress = builder.Configuration["Ocelot:DefaultAddress"];
Console.WriteLine("GW: " + ocelotUser + ", " + ocelotMessage + ", " + ocelotAddress);
Console.WriteLine("GW: " + issuer + ", " + audience + ", " + secretKey);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("MyJWT", options =>
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

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }


// app.UseSwagger();
// app.UseSwaggerUI();

// app.UseHttpsRedirection();

app.UseRouting();
app.UseOcelot().Wait();

app.UseCors("AllowAllOrigins");

app.UseAuthentication(); // Use authentication
app.UseAuthorization(); // Use authorization

app.MapControllers();

app.Run();