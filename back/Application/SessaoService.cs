using AutoMapper;

using Application.Contracts;
using Application.DTOs;
using Domain.Entities;
using Persistence.Contracts;
using Application.Helpers;

namespace Application.Services;

public class SessaoService : ISessaoService
{
    private readonly IGeralPersist _geralPersist;
    private ISessaoPersist _SessaoPersist;
    private readonly IMapper _mapper;

    public SessaoService(IGeralPersist geralPersist,
                        ISessaoPersist SessaoPersist,
                        IMapper mapper)
    {
        _geralPersist = geralPersist;
        _SessaoPersist = SessaoPersist;
        _mapper = mapper;
    }

    #region get
    public async Task<IEnumerable<SessaoDTO>> GetSessoes() {
        try {
            var Sessoes = await _SessaoPersist.GetAllSessoesAsync();
            if (Sessoes == null) 
                return null;
            var resultado = _mapper.Map<IEnumerable<SessaoDTO>>(Sessoes);
            return resultado;
        }
        catch (Exception ex) {
            throw new Exception(ex.Message);
    }}

    public async Task<SessaoDTO> GetSessaoById(int Id) {
        try {
            var Sessao = await _SessaoPersist.GetSessaoIdAsync(Id);
            if (Sessao == null) 
                return null;
            var resultado = _mapper.Map<SessaoDTO>(Sessao);
            return resultado;
        }
        catch (Exception ex) {
            throw new Exception(ex.Message);
    }}
    public async Task<IEnumerable<SessaoDTO>> GetSessoesTurma(int turmaId) {
        try {
            var Sessoes = await _SessaoPersist.GetSessoesTurmaIdAsync(turmaId);
            if (Sessoes == null) 
                return null;
            return _mapper.Map<IEnumerable<SessaoDTO>>(Sessoes);
        }
        catch (Exception ex) {
            throw new Exception(ex.Message);
    }}
    public async Task<SessaoDTO> GetSessaoAtivaTurmaIdAsync(int TurmaId){
        try {
            var sessao = await _SessaoPersist.GetSessaoAtivaTurmaIdAsync(TurmaId);
            if (sessao == null) 
                return null;
            return _mapper.Map<SessaoDTO>(sessao);
        }
        catch (Exception ex) {
            throw new Exception(ex.Message);
    }}
    public async Task<SessaoDTO> GetSessaoChavePub(string token){
        try {
            var sessoes = await _SessaoPersist.GetAllSessoesAsync();
            var sessao = sessoes.FirstOrDefault(s =>
                    s.TokenPublico == token &&
                    s.Ativo &&
                    s.DataFim == null);
            return _mapper.Map<SessaoDTO>(sessao);
        }
        catch (Exception ex) {
            throw new Exception(ex.Message);
    }}
    #endregion
    #region add
    public async Task<SessaoDTO> Add(SessaoDTO model) {
        try {
            var sessao = new Sessao
            {
                TurmaId = model.TurmaId,
                DataInicio = DateTime.UtcNow,
                TokenPublico = PublicToken.GeneratePublicToken(),
                Ativo = true
            };
            _geralPersist.Add(sessao);
            if (await _geralPersist.SaveChangesAsync()) {
                var SessaoRetorno = await _SessaoPersist.GetSessaoIdAsync(sessao.Id);
                return _mapper.Map<SessaoDTO>(SessaoRetorno);
            }
            return null;
        }
        catch (Exception ex) {
            throw new Exception(ex.Message);
    }}
    #endregion
    #region update
    public async Task<SessaoDTO> Update(int SessaoId, SessaoDTO model) {
        try {
            var Sessao = await _SessaoPersist.GetSessaoIdAsync(SessaoId);
            if (Sessao == null) return null;

            model.Id = Sessao.Id;

            _mapper.Map(model, Sessao);

            _geralPersist.Update(Sessao);

            if (await _geralPersist.SaveChangesAsync()) {
                var SessaoRetorno = await _SessaoPersist.GetSessaoIdAsync(SessaoId);

                return _mapper.Map<SessaoDTO>(SessaoRetorno);
            }
            return null;
        }
        catch (Exception ex) {
            throw new Exception(ex.Message);
    }}
    public async Task<SessaoDTO> EncerrarSessao(int SessaoId, SessaoDTO model) {
        try {
            var Sessao = await _SessaoPersist.GetSessaoIdAsync(SessaoId);
            if (Sessao == null) return null;

            model.Id = Sessao.Id;

            _mapper.Map(model, Sessao);

            Sessao.DataFim = DateTime.UtcNow;
            Sessao.Ativo = false;

            _geralPersist.Update(Sessao);

            if (await _geralPersist.SaveChangesAsync()) {
                var SessaoRetorno = await _SessaoPersist.GetSessaoIdAsync(SessaoId);

                return _mapper.Map<SessaoDTO>(SessaoRetorno);
            }
            return null;
        }
        catch (Exception ex) {
            throw new Exception(ex.Message);
    }}
    #endregion
}
