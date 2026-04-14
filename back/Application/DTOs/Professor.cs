namespace Application.DTOs;

public class ProfessorDTO
{
    public int Id { get; set; }
    public string UserName { get; set; } = null!;
    public string Nome { get; set; } = null!;
    public string SenhaHash { get; set; } = null!;    
}
