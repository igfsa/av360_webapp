namespace Domain.Entities;
public class Professor
{
    private Professor() {}
    public Professor(string userName, string senhaHash, string nome)
    {
        UserName = userName;
        SenhaHash = senhaHash;
        Nome = nome;
    }
    
    public int Id { get; private set; }
    public string UserName { get; private set; } = null!;
    public string Nome { get; private set; } = null!;
    public string SenhaHash { get; private set; } = null!;
    
}
