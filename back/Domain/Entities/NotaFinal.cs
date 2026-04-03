using Domain.Exceptions;

namespace Domain.Entities;

public class NotaFinal
{
    private NotaFinal() { }
    public NotaFinal(Sessao sessao, Aluno avaliador, Grupo grupo, string deviceHash, DateTime dataEnvio)
    {
        Sessao = sessao
            ?? throw new BusinessException("Sessão é obrigatória");
        SessaoId = sessao.Id;
        Avaliador = avaliador
            ?? throw new BusinessException("Avaliador é obrigatório");
        AvaliadorId = avaliador.Id;
        Grupo = grupo
            ?? throw new BusinessException("Grupo é obrigatório");
        GrupoId = grupo.Id;
        DeviceHash = deviceHash;
        DataEnvio = dataEnvio;
    }
    public int Id { get; private set; }
    public int SessaoId { get; private set; }
    public Sessao Sessao { get; private set; } = null!;
    public int AvaliadorId { get; private set; }
    public Aluno Avaliador { get; private set; } = null!;
    public int GrupoId { get; private set; }
    public Grupo Grupo { get; private set; } = null!;
    public string DeviceHash { get; private set; } = null!;
    public DateTime DataEnvio { get; private set; }
    private readonly List<NotaParcial> _notasParcial = [];
    public IReadOnlyCollection<NotaParcial> NotasParcial => _notasParcial;

    public void AdicionarNotaParcial(Aluno avaliado, Criterio criterio, decimal nota)
    {
        if (_notasParcial.Any(np => np.AvaliadoId == avaliado.Id && np.CriterioId == criterio.Id))
            throw new BusinessException($"{Avaliador.Nome} já avaliou {avaliado.Nome} em {criterio.Nome}.");
        if (nota < 1 || nota > Sessao.Turma.NotaMax)
            throw new BusinessException($"Nota deve estar entre 1 e {Sessao.Turma.NotaMax}");
        var np = new NotaParcial(this, avaliado, criterio, nota);
            Console.WriteLine(np);
        _notasParcial.Add(np);
    }
}