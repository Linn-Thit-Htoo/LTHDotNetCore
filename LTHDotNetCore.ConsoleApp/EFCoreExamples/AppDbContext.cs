using LTHDotNetCore.RestApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

namespace LTHDotNetCore.ConsoleApp.EFCoreExamples
{
    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            SqlConnectionStringBuilder _sqlConnectionStringBuilder = new()
            {
                DataSource = ".",
                InitialCatalog = "DotNetClass",
                UserID = "sa",
                Password = "sa@123",
                TrustServerCertificate = true
            };
            optionsBuilder.UseSqlServer(_sqlConnectionStringBuilder.ConnectionString);
        }

        public DbSet<BlogDataModel> Blogs { get; set; }
    }
}