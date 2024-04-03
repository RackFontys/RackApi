using RackApi.User.Models;
using Microsoft.EntityFrameworkCore;

namespace RackApi.User.Data;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
    {
        
    }
    public DbSet<User> Users {get;set;}
}