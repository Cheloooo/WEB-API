using Microsoft.AspNetCore.Builder;
using WEB_SERVICES;

var builder = WebApplication.CreateBuilder(args);

var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddOpenApi();
builder.Services.AddValidatorSerices();
builder.Services.AddSharedServices(builder.Configuration);

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});

var app = builder.Build();
app.UseCors("AllowSpecifiedOrigins");

//Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();