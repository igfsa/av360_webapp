using Domain.Exceptions;

namespace Domain.Entities;

public class NotaParcial
{
    private NotaParcial() { }
    public NotaParcial(NotaFinal nFinal, Aluno avaliado, Criterio criterio, decimal nota)
    {
        NotaFinal = nFinal 
            ?? throw new NotFoundException("Nota final não encontrada");
        NotaFinalId = nFinal.Id;

        Avaliado = avaliado 
            ?? throw new NotFoundException("Avaliado não encontrado");
        AvaliadoId = avaliado.Id;

        Criterio = criterio 
            ?? throw new NotFoundException("Critério não encontrado");
        CriterioId = criterio.Id;
        
        Nota = nota;
    }
    public int Id { get; private set; }
    public int NotaFinalId { get; private set; }
    public NotaFinal NotaFinal { get;  private set; } = null!;
    public int AvaliadoId { get; private set; }
    public Aluno Avaliado { get;  private set; } = null!;
    public int CriterioId { get; private set; }
    public Criterio Criterio { get;  private set; } = null!;
    public decimal Nota { get;  private set; }
}
