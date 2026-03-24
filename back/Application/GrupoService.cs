using AutoMapper;

using Application.Contracts;
using Application.DTOs;
using Domain.Entities;
using Persistence.Contracts;
using Domain.Exceptions;

namespace Application.Services;

public class GrupoService : IGrupoService
{
    private readonly IGeralPersist _geralPersist;
    private IGrupoPersist _grupoPersist;
    private ITurmaPersist _turmaPersist;
    private IAlunoGrupoPersist _alunoGrupoPersist;
    private IAlunoTurmaPersist _alunoTurmaPersist;
    private readonly IMapper _mapper;

    public GrupoService(IGeralPersist geralPersist,
                        IGrupoPersist grupoPersist,
                        ITurmaPersist turmaPersist,
                        IAlunoGrupoPersist alunoGrupoPersist,
                        IAlunoTurmaPersist alunoTurmaPersist,
                        IMapper mapper)
    {
        _geralPersist = geralPersist;
        _grupoPersist = grupoPersist;
        _turmaPersist = turmaPersist;
        _alunoGrupoPersist = alunoGrupoPersist;
        _alunoTurmaPersist = alunoTurmaPersist;
        _mapper = mapper;
    }

    #region get
    public async Task<IEnumerable<GrupoDTO>> GetGrupos() {
        try {
            var grupos = await _grupoPersist.GetAllGruposAsync()
                ?? throw new NotFoundException("Nenhum grupo encontrado");
            return _mapper.Map<IEnumerable<GrupoDTO>>(grupos);
        }
        catch {
            throw;
    }}

    public async Task<GrupoDTO> GetGrupoById(int Id) {
        try {
            var grupo = await _grupoPersist.GetGrupoIdAsync(Id)
                ?? throw new NotFoundException("Grupo não encontrado");
            return _mapper.Map<GrupoDTO>(grupo);
        }
        catch {
            throw;
    }}
    public async Task<IEnumerable<GrupoDTO>> GetGruposTurma(int turmaId) {
        try {
            var grupos = await _grupoPersist.GetGruposTurmaIdAsync(turmaId)
                ?? throw new NotFoundException("Nenhum grupo encontrado");
            return _mapper.Map<IEnumerable<GrupoDTO>>(grupos);
        }
        catch {
            throw;
    }}
    public async Task<IEnumerable<AlunoGrupoCheckboxDTO>> GetAlunoGrupoTurma(int turmaId, int grupoId) {
        try {
            var alunos = await _alunoTurmaPersist.GetAlunosTurmaIdAsync(turmaId);
            var alunoGrupos = await _alunoGrupoPersist.GetAlunosGrupoTurmaId(turmaId);
            var alunosCheckbox = alunos.Select(aluno => {
            var relacao = alunoGrupos
                .FirstOrDefault(ag => ag.AlunoId == aluno.Id);
            return new AlunoGrupoCheckboxDTO {
                AlunoId = aluno.Id,
                Nome = aluno.Nome,

                Selecionado = relacao?.GrupoId == grupoId,
                Desabilitado = relacao != null && relacao.GrupoId != grupoId,
                GrupoAtualId = relacao?.GrupoId
            };
        }).ToList();
        return alunosCheckbox;
        }
        catch {
            throw;
    }}
    // public async Task<GrupoDTO> AddAlunoGrupo(AlunoGrupoDTO model){
    //     try{
    //         var alunosAtuais = await _alunoGrupoPersist.
    //             GetAlunosGrupoId(model.grupoId);
    //         var paraRemover = alunosAtuais
    //             .Where(a => !model.alunoIds.Contains(a.Id));
    //         _geralPersist.DeleteRange(paraRemover);
    //         var idsAtuais = alunosAtuais.Select(a => a.Id).ToHashSet();
    //         var paraAdicionar = model.alunoIds
    //             .Where(id => !idsAtuais.Contains(id))
    //             .Select(id => new AlunoGrupo{
    //                 GrupoId = model.grupoId,
    //                 AlunoId = id
    //             });
    //         _geralPersist.AddRangeAsync(paraAdicionar);
    //         await _geralPersist.SaveChangesAsync();
    //         return await GetGrupoById(model.grupoId);
    //     }
    //     catch (Exception ex){
    //         throw new Exception(ex.Message);
    // }}
    #endregion
    #region add
    public async Task<GrupoDTO> Add(GrupoDTO model) {
        try {
            var turma = await _turmaPersist.GetTurmaIdAsync(model.TurmaId)
                ?? throw new NotFoundException("Turma não encontrada");
            
            var grupo = new Grupo(nome: model.Nome, turma: turma);
            
            _geralPersist.Add(grupo);
            await _geralPersist.SaveChangesAsync();
            var grupoRetorno = await _grupoPersist.GetGrupoIdAsync(grupo.Id);
            return _mapper.Map<GrupoDTO>(grupoRetorno);
        }
        catch {
            throw;
    }}
    #endregion
    #region update
    public async Task<GrupoDTO> Update(int grupoId, GrupoDTO model) {
        try {
            var grupo = await _grupoPersist.GetGrupoIdAsync(grupoId)
                ?? throw new NotFoundException("Grupo não encontrado");
            grupo.AtualizarGrupo(model.Nome);
            await _geralPersist.SaveChangesAsync();
            var grupoRetorno = await _grupoPersist.GetGrupoIdAsync(grupoId);
            return _mapper.Map<GrupoDTO>(grupoRetorno);
        }
        catch {
            throw;
    }}
    public async Task AtualizarGrupo( int turmaId, int grupoId, List<int> alunosSelecionados){
        // Remove apenas relações do grupo atual
        AlunoGrupo[] alunos = await _alunoGrupoPersist.GetAlunosGrupoTurmaId(turmaId);
        var remover = alunos.Where(ag => ag.GrupoId == grupoId);

        _geralPersist.DeleteRange(remover);

        // Adiciona os novos
        var novos = alunosSelecionados.Select(alunoId =>
            new AlunoGrupo
            {
                TurmaId = turmaId,
                GrupoId = grupoId,
                AlunoId = alunoId
            });

        _geralPersist.AddRangeAsync(novos);
        await _geralPersist.SaveChangesAsync();
    }
    #endregion
}
