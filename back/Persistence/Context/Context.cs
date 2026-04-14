using Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Persistence.Context;

public class APIContext(DbContextOptions<APIContext> options) : DbContext(options)
{
    public DbSet<Aluno> Alunos { get; set; }
    public DbSet<AlunoGrupo> AlunoGrupo { get; set; }
    public DbSet<Criterio> Criterios { get; set; }
    public DbSet<Grupo> Grupos { get; set; }
    public DbSet<NotaFinal> NotasFinais { get; set; }
    public DbSet<NotaParcial> NotasParciais { get; set; }
    public DbSet<Sessao> Sessoes { get; set; }
    public DbSet<Turma> Turmas { get; set; }
    public DbSet<Professor> Professores { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        _ = modelBuilder.Entity<Professor>()
            .HasKey(p => new {p.Id});

        _ = modelBuilder.Entity<Professor>()
            .HasIndex(p => new {p.UserName})
            .IsUnique();

        _ = modelBuilder.Entity<Professor>()
            .Property(p => p.UserName)
            .HasMaxLength(100);

        _ = modelBuilder.Entity<Professor>()
            .Property(p => p.Nome)
            .HasMaxLength(100);

        _ = modelBuilder.Entity<Professor>()
            .Property(p => p.SenhaHash)
            .IsRequired();

        _ = modelBuilder.Entity<AlunoGrupo>()
            .HasKey(ag => new { ag.AlunoId, ag.GrupoId });

        _ = modelBuilder.Entity<Aluno>()
            .Property(a => a.Nome)
            .HasMaxLength(100);

        _ = modelBuilder.Entity<Criterio>()
            .Property(c => c.Nome)
            .HasMaxLength(100);

        _ = modelBuilder.Entity<Criterio>()
            .HasData(
                new Criterio(id: 1, nome: "Nível de Participação"),
                new Criterio(id: 2, nome: "Pontualidade na Entrega de Tarefas"),
                new Criterio(id: 3, nome: "Capacidade de Trabalhar em Equipe"),
                new Criterio(id: 4, nome: "Controle Emocional"),
                new Criterio(id: 5, nome: "Disposição para Compartilhar Tarefas"),
                new Criterio(id: 6, nome: "Respeito às Individualidades"),
                new Criterio(id: 7, nome: "Responsabilidade"),
                new Criterio(id: 8, nome: "Criatividade"),
                new Criterio(id: 9, nome: "Conhecimento Teórico") 
                );

        _ = modelBuilder.Entity<Grupo>()
            .Property(g => g.Nome)
            .HasMaxLength(100);

        _ = modelBuilder.Entity<Grupo>()
            .HasOne(g => g.Turma)
            .WithMany(t => t.Grupos)
            .HasForeignKey(g => g.TurmaId)
            .OnDelete(DeleteBehavior.Restrict);

        _ = modelBuilder.Entity<Turma>()
            .Property(t => t.Cod)
            .HasMaxLength(30);

        _ = modelBuilder.Entity<Turma>()
            .Property(t => t.NotaMax)
            .HasPrecision(5, 2);

        _ = modelBuilder.Entity<Turma>()
            .HasMany(t => t.Alunos)
            .WithMany(a => a.Turmas)
            .UsingEntity(j => j.ToTable("aluno_turma"));

        _ = modelBuilder.Entity<Turma>()
            .HasMany(t => t.Criterios)
            .WithMany(c => c.Turmas)
            .UsingEntity(j => j.ToTable("criterio_turma"));

        _ = modelBuilder.Entity<Sessao>()
            .Property(s => s.TokenPublico)
            .HasMaxLength(36);

        _ = modelBuilder.Entity<NotaFinal>()
            .HasIndex(x => x.SessaoId);

        _ = modelBuilder.Entity<NotaFinal>()
            .HasIndex(a => new { a.SessaoId, a.AvaliadorId })
            .IsUnique();

        _ = modelBuilder.Entity<NotaFinal>()
            .HasIndex(a => new { a.SessaoId, a.DeviceHash })
            .IsUnique();

        _ = modelBuilder.Entity<NotaFinal>()
            .Property(n => n.DeviceHash)
            .HasMaxLength(65)
            .IsFixedLength();
        
        _ = modelBuilder.Entity<NotaParcial>()
                .HasIndex(x => x.AvaliadoId);

        _ = modelBuilder.Entity<NotaParcial>()
                .HasIndex(x => x.CriterioId);
        
        _ = modelBuilder.Entity<NotaParcial>()
            .HasIndex(a => new { a.NotaFinalId, a.AvaliadoId, a.CriterioId })
            .IsUnique();

        _ = modelBuilder.Entity<NotaParcial>()
            .HasIndex(x => new { x.AvaliadoId, x.CriterioId });

        _ = modelBuilder.Entity<NotaParcial>()
            .HasIndex(x => x.AvaliadoId)
            .IncludeProperties(x => new { x.Nota, x.CriterioId });

        _ = modelBuilder.Entity<NotaParcial>()
            .Property(n => n.Nota)
            .HasPrecision(5, 2);

        _ = modelBuilder.Entity<NotaParcial>()
            .HasOne(np => np.NotaFinal)
            .WithMany(nf => nf.NotasParcial)
            .HasForeignKey(np => np.NotaFinalId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
