using GenericRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RentCarServer.Infrastructure.Context;
using RentCarServer.Infrastructure.Options;
using Scrutor;

namespace RentCarServer.Infrastructure;

public static class ServiceRegistrar
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection service, IConfiguration configuration)
    {

        service.Configure<JwtOptions>(configuration.GetSection("Jwt"));

        service.ConfigureOptions<JwtSetupOptions>();

        service.AddAuthentication().AddJwtBearer();

        service.AddAuthorization();
        
        service.AddHttpContextAccessor();

        service.AddDbContext<ApplicationDbContext>(options =>
        {
            var con = configuration.GetConnectionString("SqlServer")!;
            options.UseSqlServer(con);
        });

        service.AddScoped<IUnitOfWork>(src => src.GetRequiredService<ApplicationDbContext>());

        service.Scan(action => action.FromAssemblies(typeof(ServiceRegistrar).Assembly).AddClasses(publicOnly: false).UsingRegistrationStrategy(RegistrationStrategy.Skip).AsImplementedInterfaces().WithScopedLifetime());

        return service;
    }
}