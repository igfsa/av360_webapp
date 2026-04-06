using AutoMapper;

using Application.Contracts;
using Application.DTOs;
using Domain.Entities;
using Persistence.Contracts;
using Domain.Exceptions;

namespace Application.Services;

public class AvaliacaoService(IGeralPersist geralPersist,
                    ISessaoService sessaoService,
                    ISessaoPersist sessaoPersist,
                    IAlunoService alunoService,
                    IAlunoPersist alunoPersist,
                    ICriterioService criterioService,
                    ICriterioPersist criterioPersist,
                    IGrupoService grupoService,
                    IGrupoPersist grupoPersist,
                    ITurmaService turmaService,
                    IDashboardCacheService dashboardCache,
                    IMapper mapper) : IAvaliacaoService
{
    private readonly IGeralPersist _geralPersist = geralPersist;
    private readonly ISessaoService _sessaoService = sessaoService;
    private readonly ISessaoPersist _sessaoPersist = sessaoPersist;
    private readonly IAlunoService _alunoService = alunoService;
    private readonly IAlunoPersist _alunoPersist = alunoPersist;
    private readonly ICriterioService _criterioService = criterioService;
    private readonly ICriterioPersist _criterioPersist = criterioPersist;
    private readonly IGrupoService _grupoService = grupoService;
    private readonly IGrupoPersist _grupoPersist = grupoPersist;
    private readonly ITurmaService _turmaService = turmaService;
    private readonly IDashboardCacheService _dashboardCache = dashboardCache;
    private readonly IMapper _mapper = mapper;

    #region get
    public async Task<AvaliacaoPublicaDTO> GetValidaSessaoChavePub(string token)
    {
        try
        {
            var sessoes = await _sessaoService.GetSessoes() ?? [];
            var sessao = sessoes.FirstOrDefault(s =>
                    s.TokenPublico == token &&
                    s.Ativo &&
                    s.DataFim == null)
                ?? throw new NotFoundException("Sessão não encontrada");
            var turma = await _turmaService.GetTurmaById(sessao.TurmaId)
                ?? throw new NotFoundException("Turma não encontrada");
            var criterios = await _criterioService.GetCriteriosTurma(sessao.TurmaId);
            var grupos = await _grupoService.GetGruposTurma(sessao.TurmaId);
            return new AvaliacaoPublicaDTO
            {
                SessaoId = sessao.Id,
                TurmaId = sessao.TurmaId,
                Turma = turma,
                Grupos = grupos ?? [],
                Criterios = criterios ?? []
            };
        }
        catch
        {
            throw;
        }
    }

    public async Task<AvaliacaoEnvioDTO> GeraNovaAvaliacaoEnvio(AvaliacaoEnvioDTO avaliacao)
    {
        try
        {
            var sessao = await _sessaoPersist.GetSessaoIdAsync(avaliacao.SessaoId)
                ?? throw new NotFoundException("Sessão inválida");
            sessao.ValidaAvaliacao(
                await _alunoPersist.GetAlunoIdAsync(avaliacao.AvaliadorId) ?? null!, 
                avaliacao.DeviceHash
                );

            var grupo = await _grupoService.GetGrupoById(avaliacao.GrupoId)
                ?? throw new NotFoundException("Grupo inválido");
            var turma = await _turmaService.GetTurmaById(grupo.TurmaId)
                ?? throw new NotFoundException("Turma inválida");

            var criterios = await _criterioService.GetCriteriosTurma(turma.Id) ?? [];
            var alunos = await _alunoService.GetAlunosGrupo(avaliacao.GrupoId) ?? [];
            avaliacao.Avaliador = alunos.FirstOrDefault(a => a.Id == avaliacao.AvaliadorId)
                ?? throw new NotFoundException("Avaliador não encontrado");
            var itensAdd = alunos.SelectMany(a =>
                criterios.Select(c => new AvaliacaoItemDTO
                {
                    AvaliadoId = a.Id,
                    Avaliado = a,
                    CriterioId = c.Id
                })
            );
            avaliacao.Itens = itensAdd;
            return avaliacao;
        }
        catch
        {
            throw;
        }
    }

    #endregion
    #region add
    public async Task<AvaliacaoPostResultDTO> AddAvaliacao(AvaliacaoEnvioDTO model)
    {
        try
        {
            _ = model.Itens
                ?? throw new NotFoundException("Sem avaliações");

            var final = new NotaFinal(
                sessao: await _sessaoPersist.GetSessaoIdAsync(model.SessaoId)
                    ?? throw new NotFoundException("Sessão inválida"),
                avaliador: await _alunoPersist.GetAlunoIdAsync(model.AvaliadorId)
                    ?? throw new NotFoundException("Aluno Avaliador não encontrado"),
                grupo: await _grupoPersist.GetGrupoIdAsync(model.GrupoId)
                    ?? throw new NotFoundException("Grupo não encontrado"),
                deviceHash: model.DeviceHash,
                dataEnvio: DateTime.UtcNow
            );
            _geralPersist.Add(final);
            Console.Write(final);
            _ = await _geralPersist.SaveChangesAsync();
            AvaliacaoPostResultDTO resultado = new();
            foreach (var np in model.Itens)
            {
                resultado.Total++;
                var aluno = await _alunoPersist.GetAlunoIdAsync(np.AvaliadoId);
                var criterio = await _criterioPersist.GetCriterioIdAsync(np.CriterioId);
                try
                {
                    if (aluno == null || criterio == null)
                    {
                        resultado.Falhas++;

                        resultado.Erros.Add(new AvaliacaoPostErrorDTO
                        {
                            Aluno = aluno?.Nome ?? "Aluno Inválido",
                            Criterio = criterio?.Nome ?? "Critério Inválido",
                            Erro = "Aluno ou critério não encontrado"
                        });

                        continue;
                    }
                    final.AdicionarNotaParcial(
                        avaliado: aluno,
                        criterio: criterio,
                        nota: np.Nota);

                    await _dashboardCache.AtualizarNotaAsync(
                        sessaoId: model.SessaoId,
                        alunoId: np.AvaliadoId,
                        criterioId: np.CriterioId,
                        grupoId: model.GrupoId,
                        nota: np.Nota
                    );

                    resultado.Sucesso++;
                }
                catch (Exception ex)
                {
                    resultado.Falhas++;
                    resultado.Erros.Add(new AvaliacaoPostErrorDTO
                    {
                        Aluno = aluno?.Nome ?? "Aluno Inválido",
                        Criterio = criterio?.Nome ?? "Critério Inválido",
                        Erro = ex.Message
                    });
                }

            }

            _ = await _geralPersist.SaveChangesAsync();
            await _dashboardCache.AtualizarAlunoAsync(model.SessaoId, model.AvaliadorId);
            
            return resultado;
        }
        catch
        {
            throw;
        }
    }
    #endregion
    #region update

    #endregion
}
