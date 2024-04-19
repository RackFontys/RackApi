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

builder.Configuration.AddJsonFile("/app/ocelot.json", false, true);
builder.Services.AddOcelot(builder.Configuration);

builder.Logging.AddConsole();

var secretKey = builder.Configuration.GetConnectionString("DefaultJWTKey");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("MyJWT", options =>
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

app.UseAuthentication(); // Use authentication
app.UseAuthorization(); // Use authorization

app.MapControllers();

app.Run();