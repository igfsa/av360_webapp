using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Exceptions;

namespace Domain.Entities;

public class Grupo
{
    private Grupo (){}
    public Grupo(string nome, Turma turma)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new BusinessException("Nome é obrigatório");
        if (turma == null)
            throw new BusinessException("Turma é obrigatória");
        Nome = nome;
        Turma = turma;
        TurmaId = turma.Id;
    }
    public int Id { get; set; }
    public string Nome { get; set; } = null!;
    public readonly Turma Turma  = null!;
    public int TurmaId { get; set; }
    public void AtualizarGrupo(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new BusinessException("Nome é obrigatório");
        Nome = nome;
    }
}
