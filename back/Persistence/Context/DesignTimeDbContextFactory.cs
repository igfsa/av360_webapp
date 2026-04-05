using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Persistence.Context;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<APIContext>
{
    public APIContext CreateDbContext(string[] args)
    {

        var server = Environment.GetEnvironmentVariable("DB_AV360_SERVER");
        var database = Environment.GetEnvironmentVariable("DB_AV360_NAME");

        var user =
            Environment.GetEnvironmentVariable("DB_PSQLUSER") ??
            Environment.GetEnvironmentVariable("DB_AV360_USER");

        var password =
            Environment.GetEnvironmentVariable("DB_PSQLPWD") ??
            Environment.GetEnvironmentVariable("DB_AV360_PASSWORD");

        if (string.IsNullOrEmpty(server) ||
            string.IsNullOrEmpty(database) ||
            string.IsNullOrEmpty(user) ||
            string.IsNullOrEmpty(password))
        {
            throw new InvalidOperationException("Variáveis de ambiente do banco não configuradas.");
        }

        var connectionString =
            $"Server={server};Database={database};Username={user};Password={password}";

        var optionsBuilder = new DbContextOptionsBuilder<APIContext>();

        optionsBuilder.UseNpgsql(connectionString)
                      .UseSnakeCaseNamingConvention();

        return new APIContext(optionsBuilder.Options);
    }
}