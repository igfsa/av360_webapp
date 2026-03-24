using AutoMapper;

using Application.Contracts;
using Application.DTOs;
using Domain.Entities;
using Persistence.Contracts;
using Application.Helpers;
using Domain.Exceptions;

namespace Application.Services;

public class SessaoService : ISessaoService
{
    private readonly IGeralPersist _geralPersist;
    private ISessaoPersist _SessaoPersist;
    private ITurmaPersist _turmaPersist;
    private readonly IMapper _mapper;

    public SessaoService(IGeralPersist geralPersist,
                        ISessaoPersist SessaoPersist,
                        ITurmaPersist turmaPersist,
                        IMapper mapper)
    {
        _geralPersist = geralPersist;
        _SessaoPersist = SessaoPersist;
        _turmaPersist = turmaPersist;
        _mapper = mapper;
    }

    #region get
    public async Task<IEnumerable<SessaoDTO>> GetSessoes() {
        try {
            var Sessoes = await _SessaoPersist.GetAllSessoesAsync()
                ?? throw new NotFoundException("Nenhuma sessão encontrada");
            return _mapper.Map<IEnumerable<SessaoDTO>>(Sessoes);
        }
        catch {
            throw;
    }}

    public async Task<SessaoDTO> GetSessaoById(int Id) {
        try {
            var Sessao = await _SessaoPersist.GetSessaoIdAsync(Id)
                ?? throw new NotFoundException("Sessão não encontrada");
            return _mapper.Map<SessaoDTO>(Sessao);
        }
        catch {
            throw;
    }}
    public async Task<IEnumerable<SessaoDTO>> GetSessoesTurma(int turmaId) {
        try {
            var Sessoes = await _SessaoPersist.GetSessoesTurmaIdAsync(turmaId)
                ?? throw new NotFoundException("Nenhuma sessão encontrada");
            return _mapper.Map<IEnumerable<SessaoDTO>>(Sessoes);
        }
        catch {
            throw;
    }}
    public async Task<SessaoDTO> GetSessaoAtivaTurmaIdAsync(int TurmaId){
        try {
            var sessao = await _SessaoPersist.GetSessaoAtivaTurmaIdAsync(TurmaId)
                ?? throw new NotFoundException("Sessão não encontrada");
            return _mapper.Map<SessaoDTO>(sessao);
        }
        catch {
            throw;
    }}
    #endregion
    #region add
    public async Task<SessaoDTO> Add(SessaoDTO model) {
        try {
            var turma = await _turmaPersist.GetTurmaIdAsync(model.TurmaId)
                ?? throw new NotFoundException("Turma não encontrada");
            var sessao = new Sessao
            (
                turma: turma,
                dataInicio: DateTime.UtcNow,
                tokenPublico: PublicToken.GeneratePublicToken()
            );
            _geralPersist.Add(sessao);
            await _geralPersist.SaveChangesAsync();
            var SessaoRetorno = await _SessaoPersist.GetSessaoIdAsync(sessao.Id);
            return _mapper.Map<SessaoDTO>(SessaoRetorno);
        }
        catch {
            throw;
    }}
    #endregion
    #region update
    public async Task<SessaoDTO> Update(int SessaoId, SessaoDTO model) {
        try {
            var Sessao = await _SessaoPersist.GetSessaoIdAsync(SessaoId);
            if (Sessao == null) 
                throw new NotFoundException("Sessão não encontrada");

            model.Id = Sessao.Id;
            _mapper.Map(model, Sessao);
            _geralPersist.Update(Sessao);

            if (await _geralPersist.SaveChangesAsync()) {
                var SessaoRetorno = await _SessaoPersist.GetSessaoIdAsync(SessaoId);

                return _mapper.Map<SessaoDTO>(SessaoRetorno);
            }
            throw new Exception("Ocorreu um erro inesperado");
        }
        catch {
            throw;
    }}
    public async Task<SessaoDTO> EncerrarSessao(int SessaoId, SessaoDTO model) {
        try {
            var Sessao = await _SessaoPersist.GetSessaoIdAsync(SessaoId)
                ?? throw new NotFoundException("Sessão não encontrada");

            Sessao.EncerrarSessao(DateTime.UtcNow);
            await _geralPersist.SaveChangesAsync();

            var SessaoRetorno = await _SessaoPersist.GetSessaoIdAsync(SessaoId);
            return _mapper.Map<SessaoDTO>(SessaoRetorno);
        }
        catch {
            throw;
    }}
    #endregion
}
