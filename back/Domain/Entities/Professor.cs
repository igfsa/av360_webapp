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

    public void AlterarPerfil(Perfil perfil) => Perfil = perfil;
    
}
