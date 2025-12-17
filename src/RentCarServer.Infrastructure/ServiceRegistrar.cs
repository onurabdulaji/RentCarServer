using GenericRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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

        service.Configure<MailSettingOptions>(configuration.GetSection("MailSettings"));
        
        using var scope = service.BuildServiceProvider().CreateScope();
        var mailSettings = scope.ServiceProvider.GetRequiredService<IOptions<MailSettingOptions>>();
        if (string.IsNullOrEmpty(mailSettings.Value.UserId))
        {
            service.AddFluentEmail(mailSettings.Value.Email)
                .AddSmtpSender(mailSettings.Value.Smtp, mailSettings.Value.Port);
        }
        else
        {
            service.AddFluentEmail(mailSettings.Value.Email)
                .AddSmtpSender(mailSettings.Value.Smtp, mailSettings.Value.Port,mailSettings.Value.UserId,mailSettings.Value.Password);
        }
        
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