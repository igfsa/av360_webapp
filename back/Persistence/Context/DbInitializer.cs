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

        Console.WriteLine("[MIGRATE] Iniciando...");

        await context.Database.MigrateAsync();

        Console.WriteLine("[SEED] Iniciando...");

        var senha = Environment.GetEnvironmentVariable("Others__Admin_Pwd");

        if (string.IsNullOrEmpty(senha))
            throw new InvalidOperationException("Others__Admin_Pwd não configurada");

        var admin = await context.Professores.FirstOrDefaultAsync(p => p.UserName == "admin01");

        if(admin is null )
        {
            var senhaHash = BCrypt.Net.BCrypt.HashPassword(senha);
            context.Professores.Add(new Professor("admin01", senhaHash, "Administrador", Perfil.Admin));
            await context.SaveChangesAsync();

            Console.WriteLine("[SEED] Admin criado com sucesso!");
        } else if (!admin.IsAdmin)
        {
            admin.AlterarPerfil(Perfil.Admin);
            await context.SaveChangesAsync();

            Console.WriteLine("[SEED] Admin alterado com sucesso!");            
        }
        else
        {
            Console.WriteLine("[SEED] Admin já existe com perfil Admin.");
        }
    }
}