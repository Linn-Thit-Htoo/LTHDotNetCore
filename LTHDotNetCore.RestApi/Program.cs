using LTHDotNetCore.MinimalApi;
using LTHDotNetCore.RestApi;
using LTHDotNetCore.RestApi.Middlewares;
using Microsoft.EntityFrameworkCore;
using NLog;
using Serilog;
using Serilog.Formatting.Compact;
using Serilog.Sinks.MSSqlServer;
using Serilog.Templates;

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

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"));
}, ServiceLifetime.Transient);

builder.Services.ConfigureLoggerService();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseLogHeaders();

app.UseHttpsRedirection();

//app.UseMiddleware<LogHeadersMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
