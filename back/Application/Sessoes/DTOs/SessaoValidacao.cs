namespace Application.DTOs;

public class SessaoValidacaoDTO
{
    public bool PodeIniciar { get; set; } = true;
    public List<SessaoValidacaoMensagemDTO> Mensagens { get; set; } = [];
}