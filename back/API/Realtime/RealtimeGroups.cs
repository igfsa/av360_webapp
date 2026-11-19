namespace API.Realtime;

public static class RealtimeGroups
{
    public static string Turma(int turmaId) => $"turma-{turmaId}";
    public static string Sessao(int sessaoId) => $"sessao-{sessaoId}";
}