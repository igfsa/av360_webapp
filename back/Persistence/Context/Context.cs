using Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Persistence.Context;

public class APIContext : DbContext
{
    public APIContext(DbContextOptions<APIContext> options) 
        : base(options) { }
    public DbSet<Aluno> Alunos { get; set; }
    public DbSet<AlunoGrupo> AlunoGrupo { get; set; }
    public DbSet<AlunoTurma> AlunoTurma { get; set; }
    public DbSet<Criterio> Criterios { get; set; }
    public DbSet<CriterioTurma> CriterioTurma { get; set; }
    public DbSet<Grupo> Grupos { get; set; }
    public DbSet<NotaFinal> NotasFinais { get; set; }
    public DbSet<NotaParcial> NotasParciais { get; set; }
    public DbSet<Sessao> Sessoes { get; set; }
    public DbSet<Turma> Turmas { get; set; }
    

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AlunoGrupo>()
                .HasKey(ag => new {ag.AlunoId, ag.GrupoId});
                
            modelBuilder.Entity<AlunoTurma>()
                .HasKey(at => new {at.AlunoId, at.TurmaId});

            modelBuilder.Entity<CriterioTurma>()
                .HasKey(ct => new {ct.CriterioId, ct.TurmaId});

            modelBuilder.Entity<Aluno>()
                .Property(a => a.Nome)
                .HasMaxLength(100);

            modelBuilder.Entity<Criterio>()
                .Property(c => c.Nome)
                .HasMaxLength(100);

            modelBuilder.Entity<Criterio>()
                .HasData(
                    new Criterio { Id = 1, Nome = "Nível de Participação" },
                    new Criterio { Id = 2, Nome = "Pontualidade na Entrega de Tarefas" },
                    new Criterio { Id = 3, Nome = "Capacidade de Trabalhar em Equipe" },
                    new Criterio { Id = 4, Nome = "Controle Emocional" },
                    new Criterio { Id = 5, Nome = "Disposição para Compartilhar Tarefas" },
                    new Criterio { Id = 6, Nome = "Respeito às Individualidades" },
                    new Criterio { Id = 7, Nome = "Responsabilidade" },
                    new Criterio { Id = 8, Nome = "Criatividade" },
                    new Criterio { Id = 9, Nome = "Conhecimento Teórico" }
                );

            modelBuilder.Entity<Grupo>()
                .Property(g => g.Nome)
                .HasMaxLength(100);

            modelBuilder.Entity<Turma>()
                .Property(t => t.Cod)
                .HasMaxLength(30);

            modelBuilder.Entity<Turma>()
                .Property(t => t.NotaMax)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Sessao>()
                .Property(s => s.TokenPublico)
                .HasMaxLength(33);

            modelBuilder.Entity<NotaFinal>()
                .HasIndex(a => new { a.SessaoId, a.AvaliadorId })
                .IsUnique();

            modelBuilder.Entity<NotaFinal>()
                .Property(n => n.DeviceHash)
                .HasMaxLength(65);

            modelBuilder.Entity<NotaParcial>()
                .HasIndex(a => new { a.NotaFinalId, a.AvaliadoId, a.CriterioId })
                .IsUnique();

            modelBuilder.Entity<NotaParcial>()
                .Property(n => n.Nota)
                .HasPrecision(5, 2);
        }
}
