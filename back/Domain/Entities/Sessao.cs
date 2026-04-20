using Domain.Exceptions;

namespace Domain.Entities;

public class Sessao
{
    private Sessao() { }
    public Sessao(Turma turma, DateTime dataInicio, string tokenPublico)
    {
        if (string.IsNullOrWhiteSpace(tokenPublico))
            throw new BusinessException("Token é obrigatório");
        Turma = turma
            ?? throw new BusinessException("Turma é obrigatória");
        TurmaId = turma.Id;
        DataInicio = dataInicio;
        TokenPublico = tokenPublico;
    }
    public int Id { get; private set; }
    public int TurmaId { get; private set; }
    public Turma Turma { get; private set; } = null!;
    public DateTime DataInicio { get; private set; }
    public DateTime? DataFim { get; private set; }
    public string TokenPublico { get; private set; } = null!;
    public bool Ativo { get; private set; } = true;
    private readonly List<NotaFinal> _notasFinais = [];
    public IReadOnlyCollection<NotaFinal> Notasfinais => _notasFinais;

    public bool ValidaAvaliacao(Aluno avaliador, string deviceHash)
    {
        if (_notasFinais.Any(nf => nf.Avaliador == avaliador))
            throw new BusinessException("Aluno já avaliou nesta sessão");
            
        Console.WriteLine(deviceHash);
        Console.WriteLine(_notasFinais.FirstOrDefault(nf => nf.DeviceHash == deviceHash));
        Console.WriteLine(_notasFinais.Any(nf => nf.DeviceHash == deviceHash));
        Console.WriteLine(_notasFinais.Count);
        
        if (_notasFinais.FirstOrDefault(nf => nf.DeviceHash == deviceHash) != null)
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
