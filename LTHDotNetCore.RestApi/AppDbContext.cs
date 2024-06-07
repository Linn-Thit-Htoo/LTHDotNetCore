using LTHDotNetCore.Models;
using Microsoft.EntityFrameworkCore;

namespace LTHDotNetCore.RestApi
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<BlogDataModel> Blogs { get; set; }
    }
}