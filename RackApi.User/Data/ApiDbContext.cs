using Microsoft.EntityFrameworkCore;
using RackApi.User.Models;

namespace RackApi.User.Data;

public class ApiDbContext : DbContext
{
    protected readonly IConfiguration _configuration;

    public ApiDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public DbSet<UserModel> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var dbHost = _configuration["DatabaseStrings:POSTGRES_HOST"];
        var dbPort = _configuration["DatabaseStrings:POSTGRES_PORT"];
        var dbDatabase = _configuration["DatabaseStrings:POSTGRES_DB"];
        var dbUsername = _configuration["DatabaseStrings:POSTGRES_USERNAME"];
        var dbPassword = _configuration["DatabaseStrings:POSTGRES_PASSWORD"];
        Console.WriteLine(dbHost);

        // connect to postgres with connection string from app settings
        options.UseNpgsql($"Host={dbHost};Port={dbPort};Database={dbDatabase};Username={dbUsername};Password={dbPassword}");
    }
}