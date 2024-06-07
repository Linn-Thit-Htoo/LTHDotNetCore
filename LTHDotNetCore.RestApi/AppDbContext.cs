using LTHDotNetCore.Models;
using Microsoft.EntityFrameworkCore;

namespace LTHDotNetCore.ConsoleApp;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<BlogDataModel> Blogs { get; set; }
}