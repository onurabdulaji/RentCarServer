using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RentCarServer.Infrastructure.Context;
using Scrutor;

namespace RentCarServer.Infrastructure;

public static class ServiceRegistrar
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection service, IConfiguration configuration)
    {
        
        service.AddHttpContextAccessor();

        service.AddDbContext<ApplicationDbContext>(options =>
        {
            var con = configuration.GetConnectionString("SqlServer")!;
            options.UseSqlServer(con);
        });

        service.Scan(action => action.FromAssemblies(typeof(ServiceRegistrar).Assembly).AddClasses(publicOnly: false).UsingRegistrationStrategy(RegistrationStrategy.Skip).AsImplementedInterfaces().WithScopedLifetime());

        return service;
    }
}