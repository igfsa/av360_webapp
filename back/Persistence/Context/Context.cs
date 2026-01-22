using Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Persistence.Context;

public class APIContext : DbContext
{
    public APIContext(DbContextOptions<APIContext> options) 
        : base(options) { }
    public DbSet<Aluno> Alunos { get; set; }
    public DbSet<Criterio> Criterios { get; set; }
    public DbSet<NotaFinal> NotasFinais { get; set; }
    public DbSet<NotaParcial> NotasParciais { get; set; }
    public DbSet<Turma> Turmas { get; set; }
    public DbSet<AlunoTurma> AlunoTurma { get; set; }
    public DbSet<CriterioTurma> CriterioTurma { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AlunoTurma>()
                .HasKey(AT => new {AT.AlunoId, AT.TurmaId});

            modelBuilder.Entity<CriterioTurma>()
                .HasKey(CT => new {CT.CriterioId, CT.TurmaId});

            modelBuilder.Entity<NotaFinal>()
                .HasOne(nf => nf.Aluno)
                .WithMany(a => a.NotasFinais)
                .HasForeignKey(nt => nt.AlunoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<NotaParcial>()
                .HasOne(np => np.Aluno)
                .WithMany(a => a.NotasParciais)
                .HasForeignKey(np => np.AlunoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
}
