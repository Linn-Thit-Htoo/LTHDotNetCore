using LTHDotNetCore.ConsoleApp;
using LTHDotNetCore.ConsoleApp.Middlewares;
using LTHDotNetCore.Services;
using Microsoft.EntityFrameworkCore;
using NLog;
using Serilog;
using Serilog.Formatting.Compact;
using Serilog.Sinks.MSSqlServer;
using Serilog.Templates;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

//var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

Log.Logger = new LoggerConfiguration()
          .MinimumLevel.Debug()
          .WriteTo.File(new CompactJsonFormatter(), "logs/RestApi.txt", rollingInterval: RollingInterval.Hour)
          .WriteTo.Console(new ExpressionTemplate(
        "[{@t:HH:mm:ss} {@l:u3} {SourceContext}] {@m}\n{@x}"))
          .WriteTo
    .MSSqlServer(
        connectionString: builder.Configuration.GetConnectionString("DbConnection"),
        sinkOptions: new MSSqlServerSinkOptions { TableName = "Tbl_Logs", AutoCreateSqlTable = true })
          .CreateLogger();

LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

builder.Host.UseSerilog();

builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.PropertyNamingPolicy = null;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"));
}, ServiceLifetime.Transient);

builder.Services.AddScoped(n =>
 new AdoDotNetService(new SqlConnectionStringBuilder(builder.Configuration.GetConnectionString("DbConnection"))));

builder.Services.AddScoped(n =>
 new DapperService(new SqlConnectionStringBuilder(builder.Configuration.GetConnectionString("DbConnection"))));


builder.Services.ConfigureLoggerService();

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        builder =>
        {
            builder.WithOrigins("https://localhost:7074", "http://localhost:5207")
                .AllowAnyMethod()
                .AllowAnyOrigin()
                .AllowAnyHeader();
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseLogHeaders();

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);


app.UseAuthorization();

app.MapControllers();

app.Run();