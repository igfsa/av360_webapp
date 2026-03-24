using Domain.Exceptions;

namespace Domain.Entities;

public class NotaFinal
{
    private NotaFinal(){}
    public NotaFinal(Sessao sessao, Aluno avaliador, Grupo grupo, string deviceHash)
    {
        if (sessao == null)
            throw new BusinessException("Sessão é obrigatória");
        if (avaliador == null)
            throw new BusinessException("Avaliador é obrigatório");
        if (grupo == null)
            throw new BusinessException("Grupo é obrigatório");
        Sessao = sessao;
        SessaoId = sessao.Id;
        Avaliador = avaliador;
        AvaliadorId = avaliador.Id;
        Grupo = grupo;
        GrupoId = grupo.Id;
        DeviceHash = deviceHash;
    }
    public int Id { get; set; }
    public int SessaoId { get; set; }
    public Sessao Sessao { get; set; } = null!;
    public int AvaliadorId { get; set; }
    public Aluno Avaliador { get; set; } = null!;
    public int GrupoId { get; set; }
    public Grupo Grupo { get; set; } = null!;
    public string DeviceHash { get; set; } = "";
    public DateTime DataEnvio { get; set; }
    private readonly List<NotaParcial> _notasParcial = new();
    public IReadOnlyCollection<NotaParcial> NotasParcial => _notasParcial;

    public void AdicionarNotaParcial(Aluno avaliado, Criterio criterio, decimal nota)
    {
        if (_notasParcial.Any(np => np.Avaliado == avaliado))
            throw new BusinessException($"{Avaliador.Nome} já avaliou {avaliado.Nome}");
        if (_notasParcial.Any(np => np.Criterio == criterio))
            throw new BusinessException($"{Avaliador.Nome} já avaliou critério {criterio.Nome}");
        if (_notasParcial.Any(np => np.Nota> 0 || np.Nota <= Sessao.Turma.NotaMax ))
            throw new BusinessException($"Nota deve estar entre 1 e {Sessao.Turma.NotaMax}");
        _notasParcial.Add(new NotaParcial(this, avaliado, criterio, nota));
    }
}