using Domain.Exceptions;

namespace Domain.Entities;
public class Professor
{
    private Professor() {}
    public Professor(string userName, string senhaHash, string nome, Perfil perfil = Perfil.Professor)
    {
        if (string.IsNullOrWhiteSpace(userName))
            throw new BusinessException("userName é obrigatório");
        if (string.IsNullOrWhiteSpace(senhaHash))
            throw new BusinessException("senhaHash é obrigatório");
        if (string.IsNullOrWhiteSpace(nome))
            throw new BusinessException("nome é obrigatório");
        UserName = userName;
        SenhaHash = senhaHash;
        Nome = nome;
        Perfil = perfil;
    }
    
    public int Id { get; private set; }
    public string UserName { get; private set; } = null!;
    public string Nome { get; private set; } = null!;
    public string SenhaHash { get; private set; } = null!;
    public Perfil Perfil { get; private set; } = Perfil.Professor;
    public bool IsAdmin => Perfil == Perfil.Admin;
    private readonly List<RefreshToken> _refreshToken = [];
    public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshToken;
    private readonly List<Turma> _turmas = [];
    public IReadOnlyCollection<Turma> Turmas => _turmas;
    private readonly List<Criterio> _criterios = [];
    public IReadOnlyCollection<Criterio> Criterios => _criterios;
    public void AlterarPerfil(Perfil perfil) => Perfil = perfil;

    public void AdicionarTurma(Turma turma)
    {
        if (_turmas.Any(a => a.Id == turma.Id))
            throw new BusinessException("Turma já matriculado.");
        _turmas.Add(turma);
    }

    public void RemoverTurma(int turmaId)
    {
        var turma = _turmas.FirstOrDefault(a => a.Id == turmaId) 
            ?? throw new BusinessException("Turma não encontrado.");
        _ = _turmas.Remove(turma);
    }

    public void AtualizarProfessor(string nome, Perfil perfil)
    {
        if (UserName == "admin01")
            throw new BusinessException("Usuário admin01 não pode ser alterado");
        Nome = nome;
        Perfil = perfil;
    }
    
}
