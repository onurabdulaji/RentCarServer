using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TS.MediatR;

namespace RentCarServer.Application;

public static class RegistrarService
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(RegistrarService).Assembly);
        });

        services.AddValidatorsFromAssembly(typeof(RegistrarService).Assembly);
        
        return services;
    }
}