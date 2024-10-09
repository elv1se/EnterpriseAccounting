using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ConsoleApp;

public class Program
{
	static void Main()
	{
		var builder = new ConfigurationBuilder();
		builder.SetBasePath(Directory.GetCurrentDirectory());
		builder.AddJsonFile("appsettings.json");
		var config = builder.Build();
		string? connectionString = config.GetConnectionString("DefaultConnection");

		var optionsBuilder = new DbContextOptionsBuilder<EnterpriseAccountingContext>();
		var options = optionsBuilder
			.UseSqlServer(connectionString)
			.Options;

		using (EnterpriseAccountingContext db = new(options))
		{

		}
	}
}