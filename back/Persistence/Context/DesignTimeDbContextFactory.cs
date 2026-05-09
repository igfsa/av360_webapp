using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Persistence.Context;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<APIContext>
{
    public APIContext CreateDbContext(string[] args)
    {

        var server = Environment.GetEnvironmentVariable("DB_AV360_SERVER");
        var database = Environment.GetEnvironmentVariable("DB_AV360_NAME");
        var user = Environment.GetEnvironmentVariable("DB_PSQLUSER");
        var password = Environment.GetEnvironmentVariable("DB_PSQLPWD");

        if (string.IsNullOrEmpty(server) ||
            string.IsNullOrEmpty(database) ||
            string.IsNullOrEmpty(user) ||
            string.IsNullOrEmpty(password))
        {
            throw new InvalidOperationException("Variáveis de ambiente do banco não configuradas.");
        }

        var connectionString =
            $"Host={server};Port=5432;Database={database};Username={user};Password={password}";
        var optionsBuilder = new DbContextOptionsBuilder<APIContext>();

        optionsBuilder.UseNpgsql(connectionString)
                      .UseSnakeCaseNamingConvention();

        return new APIContext(optionsBuilder.Options);
    }
}