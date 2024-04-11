using RackApi.Chat.Models;
using Microsoft.EntityFrameworkCore;

namespace RackApi.Chat.Data;

public class ApiDbContext : DbContext
{
    protected readonly IConfiguration Configuration;
    public ApiDbContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // connect to postgres with connection string from app settings
        options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
    }
    
    public DbSet<MessageModel> Messages {get;set;}
}