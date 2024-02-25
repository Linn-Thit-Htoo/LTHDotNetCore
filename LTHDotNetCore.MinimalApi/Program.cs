using LTHDotNetCore.MinimalApi;
using LTHDotNetCore.MinimalApi.Feaatures.Blog;
using Microsoft.EntityFrameworkCore;
using NLog;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseBlogService();

app.Run();

