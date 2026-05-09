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

        Console.WriteLine("[SEED] Iniciando...");

        var senha = Environment.GetEnvironmentVariable("AV360_ADMINPWD");

        if (string.IsNullOrEmpty(senha))
            throw new InvalidOperationException("AV360_ADMINPWD não configurada");

        if(await context.Professores.AnyAsync(p => p.UserName == "admin01"))
        {
            Console.WriteLine("[SEED] Admin já existe.");
            return;
        }

        var senhaHash = BCrypt.Net.BCrypt.HashPassword(senha);
        var professor = new Professor("admin01", senhaHash, "Administrador");

        context.Professores.Add(professor);
        await context.SaveChangesAsync();

        Console.WriteLine("[SEED] Admin criado com sucesso!");
    }
}