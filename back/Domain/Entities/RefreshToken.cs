using Domain.Exceptions;

namespace Domain.Entities;

public class RefreshToken
{
    private RefreshToken() {}
    public RefreshToken(Professor professor)
    {
        Professor = professor 
            ?? throw new NotFoundException("Professor inválido");
        ProfessorId = professor.Id;
    }
    
    public int Id { get; private set; }
    public string Token { get; private set; } =  Guid.NewGuid().ToString();
    public DateTime ExpiraEm { get; private set; } = DateTime.UtcNow.AddDays(30);
    public bool Revogado { get; private set; } = false;

    public int ProfessorId { get; private set; }
    public Professor Professor { get; private set; } = null!;

    public void Revogar()
    {
        Revogado = true;
    }
    public bool Validar()
    {
        if (ExpiraEm < DateTime.UtcNow || Revogado)
            return false;
        return true;
    }
}