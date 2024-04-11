﻿using RackApi.User.Models;
using Microsoft.EntityFrameworkCore;

namespace RackApi.User.Data;

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

    public DbSet<UserModel> Users {get;set;}
}