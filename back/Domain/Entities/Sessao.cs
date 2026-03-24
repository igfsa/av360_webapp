using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Domain.Exceptions;

namespace Domain.Entities;

public class Sessao
{
    private Sessao(){}
    public Sessao(Turma turma, DateTime dataInicio, string tokenPublico)
    {
        if (string.IsNullOrWhiteSpace(tokenPublico))
            throw new BusinessException("Token é obrigatório");
        if (turma == null)
            throw new BusinessException("Turma é obrigatória");
        TurmaId = turma.Id;
        Turma = turma;
        DataInicio = dataInicio;
        TokenPublico = tokenPublico;
    }
    public int Id { get; set; }
    public int TurmaId { get; private set; }
    public readonly Turma Turma = null!;
    public DateTime DataInicio { get; private set; }
    public DateTime? DataFim { get; private set; }
    public string TokenPublico { get; private set; } = null!;
    public bool Ativo { get; private set; }
    private readonly List<NotaFinal> _notasFinais = new();
    public IReadOnlyCollection<NotaFinal> Notasfinais => _notasFinais;

    public bool ValidaAvaliacao(Aluno avaliador, string deviceHash)
    {
        if (_notasFinais.Any(nf => nf.Avaliador == avaliador))
            throw new BusinessException("Aluno já avaliou nesta sessão");
        if (_notasFinais.Any(nf => nf.DeviceHash == deviceHash))
            throw new BusinessException("Dispositivo já usado nesta sessão");
        if (DataFim != null || !Ativo)
            throw new BusinessException("Sessão encerrada");
        return true;
    }

    public void EncerrarSessao(DateTime dataFim)
    {
        DataFim = dataFim;
        Ativo = false;
        TokenPublico = "";
    }
}
