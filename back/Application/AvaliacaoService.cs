using AutoMapper;

using Application.Contracts;
using Application.DTOs;
using Domain.Entities;
using Persistence.Contracts;
using Google.Protobuf.WellKnownTypes;
using Domain.Exceptions;

namespace Application.Services;

public class AvaliacaoService : IAvaliacaoService
{
    private readonly IGeralPersist _geralPersist;
    private readonly ISessaoService _sessaoService;
    private readonly ISessaoPersist _sessaoPersist;
    private readonly IAlunoService _alunoService;
    private readonly IAlunoPersist _alunoPersist;
    private readonly ICriterioService _criterioService;
    private readonly ICriterioPersist _criterioPersist;
    private readonly IGrupoService _grupoService;
    private readonly IGrupoPersist _grupoPersist;
    private readonly ITurmaService _turmaService;
    private readonly IMapper _mapper;

    public AvaliacaoService(IGeralPersist geralPersist,
                        ISessaoService sessaoService,
                        ISessaoPersist sessaoPersist,
                        IAlunoService alunoService,
                        IAlunoPersist alunoPersist,
                        ICriterioService criterioService,
                        ICriterioPersist criterioPersist,
                        IGrupoService grupoService,
                        IGrupoPersist grupoPersist,
                        ITurmaService turmaService,
                        IMapper mapper)
    {
        _geralPersist = geralPersist;
        _sessaoService = sessaoService;
        _sessaoPersist = sessaoPersist;
        _alunoService = alunoService;
        _alunoPersist = alunoPersist;
        _criterioService = criterioService;
        _criterioPersist = criterioPersist;
        _grupoService = grupoService;
        _grupoPersist = grupoPersist;
        _turmaService = turmaService;
        _mapper = mapper;
    }

    #region get
    public async Task<AvaliacaoPublicaDTO> GetValidaSessaoChavePub(string token){
        try {
            var sessoes = await _sessaoService.GetSessoes() ?? [];
            var sessao = sessoes.FirstOrDefault(s =>
                    s.TokenPublico == token &&
                    s.Ativo &&
                    s.DataFim == null)
                ?? throw new NotFoundException("Sessão não encontrada");
            var criterios = await _criterioService.GetCriteriosTurma(sessao.TurmaId);
            var grupos = await _grupoService.GetGruposTurma(sessao.TurmaId);
            return new AvaliacaoPublicaDTO{
                SessaoId = sessao.Id,
                TurmaId = sessao.TurmaId,
                Grupos = grupos ?? [],
                Criterios = criterios ?? []
            };
        }
        catch {
            throw;
    }}

    public async Task<AvaliacaoEnvioDTO> GeraNovaAvaliacaoEnvio(AvaliacaoEnvioDTO avaliacao) {
        try{
            var sessao = await _sessaoService.GetSessaoById(avaliacao.SessaoId) 
                ?? throw new Exception("Sessão inválida");
            var grupo = await _grupoService.GetGrupoById(avaliacao.GrupoId) 
                ?? throw new Exception("Grupo inválido");
            var turma = await _turmaService.GetTurmaById(grupo.TurmaId) 
                ?? throw new Exception("Turma inválida");
            
            var criterios = await _criterioService.GetCriteriosTurma(turma.Id) ?? [];
            var alunos = await _alunoService.GetAlunosGrupo(avaliacao.GrupoId) ?? [];
            var itensAdd = alunos.SelectMany(a => 
                criterios.Select(c => new AvaliacaoItemDTO
                {
                    AvaliadoId = a.Id,
                    CriterioId = c.Id,
                    Nota = 0
                })
            );
            avaliacao.Itens = itensAdd;
            return avaliacao;
        }
        catch {
            throw;
    }}

    #endregion
    #region add
    public async Task<NotaFinalDTO> AddAvaliacao(AvaliacaoEnvioDTO model){
        try{
            if (model.Itens == null)
                throw new BusinessException("Sem avaliações");

            var final = new NotaFinal(
                sessao: await _sessaoPersist.GetSessaoIdAsync(model.SessaoId) 
                    ?? throw new BusinessException("Sessão inválida"),
                avaliador: await _alunoPersist.GetAlunoIdAsync(model.AvaliadorId) 
                    ?? throw new BusinessException("Avaliador não encontrado"),
                grupo: await _grupoPersist.GetGrupoIdAsync(model.GrupoId) 
                    ?? throw new BusinessException("Grupo não encontrado"),
                deviceHash: model.DeviceHash
            );
            _geralPersist.Add(final);
            await _geralPersist.SaveChangesAsync();
            var paraAdicionar = model.Itens
                .Select(async np => 
                    final.AdicionarNotaParcial(
                        avaliado: await _alunoPersist.GetAlunoIdAsync(np.AvaliadoId)
                            ?? throw new BusinessException("Avaliado não encontrado"),
                        criterio: await _criterioPersist.GetCriterioIdAsync(np.CriterioId)
                            ?? throw new BusinessException("Critério não encontrado"),
                        nota: np.Nota ))
                .ToList();
            _geralPersist.AddRangeAsync(paraAdicionar);
            await _geralPersist.SaveChangesAsync();
            return _mapper.Map<NotaFinalDTO>(final);
        }
        catch {
            throw;
    }}
    #endregion
    #region update

    #endregion
}
