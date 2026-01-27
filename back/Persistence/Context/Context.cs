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

            modelBuilder.Entity<NotaFinal>()
                .HasIndex(a => new { a.SessaoId, a.AvaliadorId })
                .IsUnique();

            modelBuilder.Entity<NotaFinal>()
                .HasIndex(a => new { a.SessaoId, a.DeviceHash })
                .IsUnique();
        }
}
