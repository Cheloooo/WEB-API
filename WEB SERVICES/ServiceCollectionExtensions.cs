using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WEB.DAL.AppDbContext;

namespace WEB_SERVICES;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddDataAccess(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction)
	{
		services.AddDbContext<WebApiDbContext>(optionsAction);
		return services;
	}
}