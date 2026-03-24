using Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Persistence.Context;

public class APIContext : DbContext
{
    public APIContext(DbContextOptions<APIContext> options) 
        : base(options) { }
    public DbSet<Aluno> Alunos { get; set; }
    public DbSet<AlunoGrupo> AlunoGrupo { get; set; }
    public DbSet<Criterio> Criterios { get; set; }
    public DbSet<Grupo> Grupos { get; set; }
    public DbSet<NotaFinal> NotasFinais { get; set; }
    public DbSet<NotaParcial> NotasParciais { get; set; }
    public DbSet<Sessao> Sessoes { get; set; }
    public DbSet<Turma> Turmas { get; set; }
    

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AlunoGrupo>()
                .HasKey(ag => new {ag.AlunoId, ag.GrupoId});

            modelBuilder.Entity<Aluno>()
                .Property(a => a.Nome)
                .HasMaxLength(100);

            modelBuilder.Entity<Criterio>()
                .Property(c => c.Nome)
                .HasMaxLength(100);

            modelBuilder.Entity<Criterio>()
                .HasData(
                    new Criterio (nome: "Nível de Participação" ) { Id = 1 },
                    new Criterio (nome: "Pontualidade na Entrega de Tarefas"){ Id = 2 },
                    new Criterio (nome: "Capacidade de Trabalhar em Equipe"){ Id = 3 },
                    new Criterio (nome: "Controle Emocional"){ Id = 4 },
                    new Criterio (nome: "Disposição para Compartilhar Tarefas"){ Id = 5 },
                    new Criterio (nome: "Respeito às Individualidades"){ Id = 6 },
                    new Criterio (nome: "Responsabilidade"){ Id = 7 },
                    new Criterio (nome: "Criatividade"){ Id = 8 },
                    new Criterio (nome: "Conhecimento Teórico"){ Id = 9 }
                );

            modelBuilder.Entity<Grupo>()
                .Property(g => g.Nome)
                .HasMaxLength(100);

            modelBuilder.Entity<Grupo>()
                .HasOne(g => g.Turma)
                .WithMany(t => t.Grupos)
                .HasForeignKey(g => g.TurmaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Turma>()
                .Property(t => t.Cod)
                .HasMaxLength(30);

            modelBuilder.Entity<Turma>()
                .Property(t => t.NotaMax)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Turma>()
                .HasMany(t => t.Alunos)
                .WithMany(a => a.Turmas)
                .UsingEntity(j => j.ToTable("AlunoTurma"));

            modelBuilder.Entity<Turma>()
                .HasMany(t => t.Criterios)
                .WithMany(c => c.Turmas)
                .UsingEntity(j => j.ToTable("CriterioTurma"));
                
            modelBuilder.Entity<Sessao>()
                .Property(s => s.TokenPublico)
                .HasMaxLength(33);

            modelBuilder.Entity<NotaFinal>()
                .HasIndex(a => new { a.SessaoId, a.AvaliadorId })
                .IsUnique();

            modelBuilder.Entity<NotaFinal>()
                .HasIndex(a => new { a.SessaoId, a.DeviceHash })
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
