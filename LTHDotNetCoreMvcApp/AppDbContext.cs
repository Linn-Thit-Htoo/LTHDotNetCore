using LTHDotNetCoreMvcApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LTHDotNetCoreMvcApp
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<BlogDataModel> Blogs { get; set; }
        public DbSet<LoginDataModel> Login { get; set; }
    }
}
