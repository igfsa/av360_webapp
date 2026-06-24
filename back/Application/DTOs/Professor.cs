using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public class ProfessorDTO
{
    public int Id { get; set; }

    [RegularExpression(@"^[a-zA-Z0-9]+$")]
    [MaxLength(100)]
    public string UserName { get; set; } = null!;

    [MaxLength(100)]
    public string Nome { get; set; } = null!;

    [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z\d]).{8,}$",
        ErrorMessage = "A senha deve possuir pelo menos 8 caracteres, incluindo letra maiúscula, minúscula, número e caractere especial."
    )]
    [Length(8, 50)]
    public string SenhaHash { get; set; } = null!;    
}
