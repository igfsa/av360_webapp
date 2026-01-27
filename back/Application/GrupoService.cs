using AutoMapper;

using Application.Contracts;
using Application.DTOs;
using Domain.Entities;
using Persistence.Contracts;

namespace Application.Services;

public class GrupoService : IGrupoService
{
    private readonly IGeralPersist _geralPersist;
    private IGrupoPersist _grupoPersist;
    private IAlunoGrupoPersist _alunoGrupoPersist;
    private readonly IMapper _mapper;

    public GrupoService(IGeralPersist geralPersist,
                        IGrupoPersist grupoPersist,
                        IAlunoGrupoPersist alunoGrupoPersist,
                        IMapper mapper)
    {
        _geralPersist = geralPersist;
        _grupoPersist = grupoPersist;
        _alunoGrupoPersist = alunoGrupoPersist;
        _mapper = mapper;
    }

    #region get
    public async Task<IEnumerable<GrupoDTO>> GetGrupos() {
        try {
            var grupos = await _grupoPersist.GetAllGruposAsync();
            if (grupos == null) 
                return null;
            var resultado = _mapper.Map<IEnumerable<GrupoDTO>>(grupos);
            return resultado;
        }
        catch (Exception ex) {
            throw new Exception(ex.Message);
    }}

    public async Task<GrupoDTO> GetGrupoById(int Id) {
        try {
            var grupo = await _grupoPersist.GetGrupoIdAsync(Id);
            if (grupo == null) 
                return null;
            var resultado = _mapper.Map<GrupoDTO>(grupo);
            return resultado;
        }
        catch (Exception ex) {
            throw new Exception(ex.Message);
    }}
    public async Task<IEnumerable<GrupoDTO>> GetGruposTurma(int turmaId) {
        try {
            var grupos = await _grupoPersist.GetGruposTurmaIdAsync(turmaId);
            if (grupos == null) 
                return null;
            return _mapper.Map<IEnumerable<GrupoDTO>>(grupos);
        }
        catch (Exception ex) {
            throw new Exception(ex.Message);
    }}
    public async Task<GrupoDTO> AddAlunoGrupo(AlunoGrupoDTO model){
        try{
            var alunosAtuais = await _alunoGrupoPersist.
                GetAlunosGrupoId(model.grupoId);
            var paraRemover = alunosAtuais
                .Where(a => !model.alunoIds.Contains(a.Id));
            _geralPersist.DeleteRange(paraRemover);
            var idsAtuais = alunosAtuais.Select(a => a.Id).ToHashSet();
            var paraAdicionar = model.alunoIds
                .Where(id => !idsAtuais.Contains(id))
                .Select(id => new AlunoGrupo{
                    GrupoId = model.grupoId,
                    AlunoId = id
                });
            _geralPersist.AddRangeAsync(paraAdicionar);
            await _geralPersist.SaveChangesAsync();
            return await GetGrupoById(model.grupoId);
        }
        catch (Exception ex){
            throw new Exception(ex.Message);
    }}
    #endregion
    #region add
    public async Task<GrupoDTO> Add(GrupoDTO model) {
        try {
            var grupo = _mapper.Map<Grupo>(model);
            _geralPersist.Add(grupo);
            if (await _geralPersist.SaveChangesAsync()) {
                var grupoRetorno = await _grupoPersist.GetGrupoIdAsync(grupo.Id);
                return _mapper.Map<GrupoDTO>(grupoRetorno);
            }
            return null;
        }
        catch (Exception ex) {
            throw new Exception(ex.Message);
    }}
    #endregion
    #region update
    public async Task<GrupoDTO> Update(int grupoId, GrupoDTO model) {
        try {
            var grupo = await _grupoPersist.GetGrupoIdAsync(grupoId);
            if (grupo == null) return null;

            model.Id = grupo.Id;

            _mapper.Map(model, grupo);

            _geralPersist.Update(grupo);

            if (await _geralPersist.SaveChangesAsync()) {
                var grupoRetorno = await _grupoPersist.GetGrupoIdAsync(grupoId);

                return _mapper.Map<GrupoDTO>(grupoRetorno);
            }
            return null;
        }
        catch (Exception ex) {
            throw new Exception(ex.Message);
    }}
    #endregion
}
