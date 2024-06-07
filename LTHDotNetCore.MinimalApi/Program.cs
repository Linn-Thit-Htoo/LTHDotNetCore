using LTHDotNetCore.MinimalApi;
using LTHDotNetCore.MinimalApi.Features.Blog;
using LTHDotNetCore.Services;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = null;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"));
});

builder.Services.AddScoped(n =>
new AdoDotNetService(
    new SqlConnectionStringBuilder(builder.Configuration.GetConnectionString("DbConnection"))));

builder.Services.AddScoped(n =>
new DapperService(
    new SqlConnectionStringBuilder(builder.Configuration.GetConnectionString("DbConnection"))));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseBlogService();
//app.UseBlogAdoDotNetService();
app.UseDapperService();

app.Run();