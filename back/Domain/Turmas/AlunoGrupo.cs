namespace Domain.Entities;

public class AlunoGrupo(int alunoId, int grupoId, int turmaId)
{
    public int AlunoId { get; private set; } = alunoId;
    public int GrupoId { get; private set; } = grupoId;
    public int TurmaId { get; private set; } = turmaId;
}
