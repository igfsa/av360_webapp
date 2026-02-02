using AutoMapper;

using Application.Contracts;
using Application.DTOs;
using Domain.Entities;
using Persistence.Contracts;
using Mysqlx;
using Persistence;

namespace Application.Services;

public class AvaliacaoService : IAvaliacaoService
{
    private readonly IGeralPersist _geralPersist;
    private readonly ISessaoService _sessaoService;
    private readonly IAlunoGrupoPersist _alunoGrupoPersist;
    private readonly INotaFinalPersist _notaFinalPersist;    
    private readonly IMapper _mapper;

    public AvaliacaoService(IGeralPersist geralPersist,
                        ISessaoService sessaoService,
                        IAlunoGrupoPersist alunoGrupoPersist,
                        INotaFinalPersist notaFinalPersist,
                        IMapper mapper)
    {
        _geralPersist = geralPersist;
        _sessaoService = sessaoService;
        _alunoGrupoPersist = alunoGrupoPersist;
        _notaFinalPersist = notaFinalPersist;
        _mapper = mapper;
    }

    #region get

    #endregion
    #region add
    public async Task<NotaFinalDTO> AddAvaliacao(AvaliacaoEnvioDTO model){
        try{
            if (_alunoGrupoPersist.GetExisteAlunoGrupo(model.GrupoId, model.Avaliador) == null)
                throw new Exception("Aluno não inserido no grupo");
            if (_notaFinalPersist.GetNotaFinalAlunoSessaoIdAsync(model.Avaliador, model.SessaoId) != null
                || _notaFinalPersist.GetNotaFinalHashAsync(model.DeviceHash, model.SessaoId) != null
            )
                throw new Exception("Aluno/Dispositivo já avaliou nesta sessão");

            var sessao = await _sessaoService.GetSessaoById(model.SessaoId);
            if (sessao.DataFim != null)
                throw new Exception("Prazo encerrado");

            var final = new NotaFinal{
                SessaoId = model.SessaoId,
                AvaliadorId = model.Avaliador,
                GrupoId = model.GrupoId,
                DeviceHash = model.DeviceHash,
                DataEnvio = DateTime.Now
            };
            await _geralPersist.SaveChangesAsync();
            var paraAdicionar = model.Itens
                .Select(np => 
                    new NotaParcial{
                        NotaFinalId = final.Id,
                        AvaliadoId = np.AvaliadoId,
                        CriterioId = np.CriterioId,
                        Nota = np.Nota })
                .ToList();
            _geralPersist.AddRangeAsync(paraAdicionar);
            await _geralPersist.SaveChangesAsync();
            return _mapper.Map<NotaFinalDTO>(final);
        }
        catch (Exception ex){
            throw new Exception(ex.Message);
    }}
    #endregion
    #region update

    #endregion
}
