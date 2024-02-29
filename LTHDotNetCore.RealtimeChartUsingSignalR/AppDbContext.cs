using LTHDotNetCore.RealtimeChartUsingSignalR.Models;
using Microsoft.EntityFrameworkCore;

namespace LTHDotNetCore.RealtimeChartUsingSignalR
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<TeamDataModel> Teams { get; set; }
    }
}
