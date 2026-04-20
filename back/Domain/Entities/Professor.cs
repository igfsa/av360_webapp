using Domain.Exceptions;

namespace Domain.Entities;
public class Professor
{
    private Professor() {}
    public Professor(string userName, string senhaHash, string nome)
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
    }
    
    public int Id { get; private set; }
    public string UserName { get; private set; } = null!;
    public string Nome { get; private set; } = null!;
    public string SenhaHash { get; private set; } = null!;
    private readonly List<RefreshToken> _refreshToken = [];
    public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshToken;
    
}
