using Microsoft.VisualBasic;
using Serilog;
using WEB_GATEWAY;
using WEB_GATEWAY.Models;
using WEB_UTILITY.Logger;
using Yarp.ReverseProxy.Transforms;
using Constants = WEB_GATEWAY.Constants;

var builder = WebApplication.CreateBuilder(args);

var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
builder.Services.AddCors(options =>
{
   options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Configuration.AddEnvironmentVariables();
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettin.json", optional: true, reloadOnChange: true);
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.{builder.Environment.EnvirontmentName}.json", optional: true, reloadOnChange: true);

Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).Enrich.FromLogContext().CreateLogger();

builder.Host.UseSerilog();

Log.ForContext<Program>().Information("API Gateway Initialized in {Environment} environment {Date} on {Urls}",
    builder.Environment.EnvironmentName, 
    DateTime.UtcNow.ToUniversalTime(), 
    builder.Configuration.GetValue<string>("ASPNETCORE_URLS") ?? "unset"
    );

builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton(typeof(IAppLogger<>), typeof(AppLogger<>));

Log.ForContext<Program>().Information("Setting up Reverse Proxy");


//gateway settings

builder.Services.Configure<ReverseProxy>(builder.Configuration.GetSection(Constants.REVERSE_PROXY));
builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection(Constants.REVERSE_PROXY))
    .AddTransforms(builderContext =>
    {
        builderContext.AddRequestTransform(async transformContext =>
        {

        });
    });