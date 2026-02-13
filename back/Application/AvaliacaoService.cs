using AutoMapper;

using Application.Contracts;
using Application.DTOs;
using Domain.Entities;
using Persistence.Contracts;
using Google.Protobuf.WellKnownTypes;

namespace Application.Services;

public class AvaliacaoService : IAvaliacaoService
{
    private readonly IGeralPersist _geralPersist;
    private readonly ISessaoService _sessaoService;
    private readonly IAlunoGrupoPersist _alunoGrupoService;
    private readonly IAlunoService _alunoService;
    private readonly ICriterioService _criterioService;
    private readonly INotaFinalPersist _notaFinalPersist;    
    private readonly IGrupoService _grupoService;
    private readonly ITurmaService _turmaService;
    private readonly IMapper _mapper;

    public AvaliacaoService(IGeralPersist geralPersist,
                        ISessaoService sessaoService,
                        IAlunoGrupoPersist alunoGrupoService,
                        IAlunoService alunoService,
                        ICriterioService criterioService,
                        INotaFinalPersist notaFinalPersist,
                        IGrupoService grupoService,
                        ITurmaService turmaService,
                        IMapper mapper)
    {
        _geralPersist = geralPersist;
        _sessaoService = sessaoService;
        _alunoGrupoService = alunoGrupoService;
        _alunoService = alunoService;
        _criterioService = criterioService;
        _notaFinalPersist = notaFinalPersist;
        _grupoService = grupoService;
        _turmaService = turmaService;
        _mapper = mapper;
    }

    #region get
    public async Task<AvaliacaoPublicaDTO?> GetValidaSessaoChavePub(string token){
        try {
            var sessoes = await _sessaoService.GetSessoes() ?? [];
            var sessao = sessoes.FirstOrDefault(s =>
                    s.TokenPublico == token &&
                    s.Ativo &&
                    s.DataFim == null);
            if (sessao == null)
                return null;
            var criterios = await _criterioService.GetCriteriosTurma(sessao.TurmaId);
            var grupos = await _grupoService.GetGruposTurma(sessao.TurmaId);
            return new AvaliacaoPublicaDTO{
                SessaoId = sessao.Id,
                TurmaId = sessao.TurmaId,
                Grupos = grupos ?? [],
                Criterios = criterios ?? []
            };
        }
        catch (Exception ex) {
            throw new Exception(ex.Message);
    }}

    public async Task<AvaliacaoEnvioDTO> GeraNovaAvaliacaoEnvio(AvaliacaoEnvioDTO avaliacao) {
        try{
            var grupo = await _grupoService.GetGrupoById(avaliacao.GrupoId) ?? throw new Exception("Grupo inválido");
            var turma = await _turmaService.GetTurmaById(grupo.TurmaId) ?? throw new Exception("Turma inválida");
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
        catch (Exception ex) {
            throw new Exception(ex.Message);
    }}

    #endregion
    #region add
    public async Task<NotaFinalDTO> AddAvaliacao(AvaliacaoEnvioDTO model){
        try{
            if (await _alunoGrupoService.GetExisteAlunoGrupo(model.GrupoId, model.AvaliadorId) == null)
                throw new Exception("Aluno não inserido no grupo");
            if (await _notaFinalPersist.GetNotaFinalAlunoSessaoIdAsync(model.AvaliadorId, model.SessaoId) != null)
                throw new Exception("Aluno já avaliou nesta sessão");
            if(await _notaFinalPersist.GetNotaFinalHashAsync(model.DeviceHash, model.SessaoId) != null)
                throw new Exception("Dispositivo já avaliou nesta sessão");

            var sessao = await _sessaoService.GetSessaoById(model.SessaoId) ?? throw new Exception("Sessão inválida");
            if (sessao.DataFim != null)
                throw new Exception("Prazo encerrado");
            if (model.Itens == null)
                throw new Exception("Sem avaliações");

            var final = new NotaFinal{
                SessaoId = model.SessaoId,
                AvaliadorId = model.AvaliadorId,
                GrupoId = model.GrupoId,
                DeviceHash = model.DeviceHash,
                DataEnvio = DateTime.Now
            };
            _geralPersist.Add(final);
            await _geralPersist.SaveChangesAsync();
            Console.WriteLine($"-------------------------------------------------{final.Id}-------------------------------------------------");
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
