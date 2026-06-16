using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Persistence.Context;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<APIContext>
{
    public APIContext CreateDbContext(string[] args)
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<APIContext>();

        optionsBuilder
            .UseNpgsql(Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection"))
            .UseSnakeCaseNamingConvention();

        return new APIContext(optionsBuilder.Options);
    }
}