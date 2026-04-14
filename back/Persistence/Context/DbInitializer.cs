using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Persistence.Context;

public static class DbInitializer
{
    public static async Task Seed(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<APIContext>();

        if (await context.Professores.FirstOrDefaultAsync(p => p.UserName == "admin01") != null)
            return;

        var senhaHash = BCrypt.Net.BCrypt.HashPassword(Environment.GetEnvironmentVariable("AV360_ADMINPWD"));
        var professor = new Professor("admin01", senhaHash, "Administrador");

        context.Professores.Add(professor);
        await context.SaveChangesAsync();
    }
}