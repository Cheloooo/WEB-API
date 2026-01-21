using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WEB.DAL;
using WEB.DAL.AppDbContext;
using WEB.DAL.Repository;
using WEB_DOMAIN.Interface;
using WEB_SERVICES.MappingProfile.Authentication;
using WEB_SERVICES.MappingProfile.Generic;
using WEB_UTILITY.Logger;
using WEB_UTILITY.Security;
using WEB_UTILITY.Security.ISecurity;

namespace WEB_SERVICES;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSharedServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<AuthProfile>();
            cfg.AddProfile<UserProfile>();
        });
        services.AddTransient<IgnoreAuthInClientMapping>();
        //you use transient  created when requested, if a controller needs it 5x the controller gets 5 different instances every call gets its own instance, example: lightweight stateless services
        services.AddMemoryCache();

        services.AddScoped<PasswordEncryptionResolver>();
        //one instance per http request  if a controller needs it 5x the controller gets the same instance every call within that http request, example : db context, user session
        var rsaKeyManager = RsaKeyManager.Instance;
        rsaKeyManager.LoadPublicKey(configuration["RsaKeys:Public"]!);
        rsaKeyManager.LoadPrivateKey(configuration["RsaKeys:Private"]!);
        services.AddSingleton<RsaKeyManager>(rsaKeyManager);
        //created once for the lifetime of the application (best for stateless services) example: configuration service, logging service
        services.AddHttpContextAccessor();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped(typeof(IBaseEntityRepository<>), typeof(BaseEntityRepository<>));
        services.AddScoped<IUnitofWork, UnitOfWork>();
        services.AddSingleton(typeof(IAppLogger<>), typeof(AppLogger<>));
        services.AddScoped<IRsaEncryptionService, RsaEncryptionService>();
        services.AddTransient<GetSessionResolver>();
        return services;

    }
    public static IServiceCollection AddDataAccess(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction)
	{
		services.AddDbContext<WebApiDbContext>(optionsAction);
		return services;
	}
}