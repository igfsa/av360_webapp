namespace Application.DTOs;

public class RefreshTokenDTO
{
    public int Id { get; set; }
    public string Token { get; set; } = null!;
    public DateTime ExpiraEm { get; set; }
    public bool Revogado { get; set; }

    public int ProfessorId { get; set; }
    public ProfessorDTO Professor { get; set; } = null!;
}