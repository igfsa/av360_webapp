using AutoMapper;

using Application.Contracts;
using Application.DTOs;
using Domain.Entities;
using Persistence.Contracts;
using Application.Helpers;
using Domain.Exceptions;

namespace Application.Services;

public class SessaoService(IGeralPersist geralPersist,
                    ISessaoPersist SessaoPersist,
                    ITurmaPersist turmaPersist,
                    IMapper mapper) : ISessaoService
{
    private readonly IGeralPersist _geralPersist = geralPersist;
    private readonly ISessaoPersist _sessaoPersist = SessaoPersist;
    private readonly ITurmaPersist _turmaPersist = turmaPersist;
    private readonly IMapper _mapper = mapper;

    #region get
    public async Task<SessaoDTO> GetSessaoById(int Id)
    {
        try
        {
            var Sessao = await _sessaoPersist.GetSessaoIdAsync(Id)
                ?? throw new NotFoundException("Sessão não encontrada");
            return _mapper.Map<SessaoDTO>(Sessao);
        }
        catch
        {
            throw;
        }
    }
    public async Task<SessaoDTO> GetSessaoAtivaTurmaIdAsync(int TurmaId)
    {
        try
        {
            var sessao = await _sessaoPersist.GetSessaoAtivaTurmaIdAsync(TurmaId)
                ?? null!;
            return _mapper.Map<SessaoDTO>(sessao);
        }
        catch
        {
            throw;
        }
    }
    #endregion
    #region add
    public async Task<SessaoDTO> Add(SessaoDTO model)
    {
        try
        {
            var turma = await _turmaPersist.GetTurmaIdAsync(model.TurmaId)
                ?? throw new NotFoundException("Turma não encontrada");
            var sessao = new Sessao
            (
                turma: turma,
                dataInicio: DateTime.UtcNow,
                tokenPublico: PublicToken.GeneratePublicToken()
            );
            _geralPersist.Add(sessao);
            _ = await _geralPersist.SaveChangesAsync();
            var SessaoRetorno = await _sessaoPersist.GetSessaoIdAsync(sessao.Id);
            return _mapper.Map<SessaoDTO>(SessaoRetorno);
        }
        catch
        {
            throw;
        }
    }
    #endregion
    #region update
    public async Task<SessaoDTO> EncerrarSessao(int SessaoId, SessaoDTO model)
    {
        try
        {
            var Sessao = await _sessaoPersist.GetSessaoIdAsync(SessaoId)
                ?? throw new NotFoundException("Sessão não encontrada");

            Sessao.EncerrarSessao(DateTime.UtcNow);
            _ = await _geralPersist.SaveChangesAsync();

            var SessaoRetorno = await _sessaoPersist.GetSessaoIdAsync(SessaoId);
            return _mapper.Map<SessaoDTO>(SessaoRetorno);
        }
        catch
        {
            throw;
        }
    }
    #endregion
}
