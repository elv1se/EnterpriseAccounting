using Contracts;
using EnterpriseAccounting.Application;
using EnterpriseAccounting.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseAccounting.WebMVC.Extensions;

public static class ServiceExtensions
{
	public static void ConfigureSqlContext(this IServiceCollection services,
		IConfiguration configuration) =>
		services.AddDbContext<EnterpriseAccountingContext>(opts =>
			opts.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), b =>
			{
				b.EnableRetryOnFailure();
			})
		);

	public static void ConfigureRepositoryManager(this IServiceCollection services) =>
		services.AddScoped<IRepositoryManager, RepositoryManager>();

	public static void ConfigureServiceManager(this IServiceCollection services) =>
		services.AddScoped<IServiceManager, ServiceManager>();
}
