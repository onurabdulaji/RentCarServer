using System.Threading.RateLimiting;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.RateLimiting;
using RentCarServer.Application;
using RentCarServer.Infrastructure;
using RentCarServer.WebApi;
using RentCarServer.WebApi.Modules;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddRateLimiter(cfg =>
{
    cfg.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.PermitLimit = 100;
        opt.QueueLimit = 100;
        opt.Window = TimeSpan.FromSeconds(1);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });

    cfg.AddFixedWindowLimiter("login-fixed", opt =>
    {
        opt.PermitLimit = 5;
        opt.QueueLimit = 1;
        opt.Window = TimeSpan.FromMinutes(1);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
});

builder.Services.AddControllers().AddOData(opt =>
{
    opt.Select()
       .Filter()
       .Count()
       .Expand()
       .OrderBy()
       .SetMaxTop(null);
});

builder.Services.AddCors();

builder.Services.AddOpenApi();

builder.Services.AddExceptionHandler<ExceptionHandler>().AddProblemDetails();

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});

var app = builder.Build();

app.MapOpenApi();

app.MapScalarApiReference();

app.UseHttpsRedirection();

app.UseCors(opt =>
{
    opt.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod().SetPreflightMaxAge(TimeSpan.FromMinutes(10));
});

app.UseRequestDecompression();

app.UseAuthentication();

app.UseAuthorization();

app.UseRateLimiter();

app.UseExceptionHandler();

app.MapControllers().RequireRateLimiting("fixed").RequireAuthorization();

app.MapAuth();

app.MapGet("/", () => "Hello World").RequireAuthorization();

//await app.CreateFirstUser();

app.Run();
