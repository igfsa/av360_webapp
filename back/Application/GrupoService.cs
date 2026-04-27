using AutoMapper;

using Application.Contracts;
using Application.DTOs;
using Domain.Entities;
using Persistence.Contracts;
using Domain.Exceptions;

namespace Application.Services;

public class GrupoService(IGeralPersist geralPersist,
                    IGrupoPersist grupoPersist,
                    ITurmaPersist turmaPersist,
                    IAlunoGrupoPersist alunoGrupoPersist,
                    IAlunoTurmaPersist alunoTurmaPersist,
                    IMapper mapper) : IGrupoService
{
    private readonly IGeralPersist _geralPersist = geralPersist;
    private readonly IGrupoPersist _grupoPersist = grupoPersist;
    private readonly ITurmaPersist _turmaPersist = turmaPersist;
    private readonly IAlunoGrupoPersist _alunoGrupoPersist = alunoGrupoPersist;
    private readonly IAlunoTurmaPersist _alunoTurmaPersist = alunoTurmaPersist;
    private readonly IMapper _mapper = mapper;

    #region get
    public async Task<GrupoDTO> GetGrupoById(int Id)
    {
        try
        {
            var grupo = await _grupoPersist.GetGrupoIdAsync(Id)
                ?? throw new NotFoundException("Grupo não encontrado");
            return _mapper.Map<GrupoDTO>(grupo);
        }
        catch
        {
            throw;
        }
    }
    public async Task<IEnumerable<GrupoDTO>> GetGruposTurma(int turmaId)
    {
        try
        {
            var grupos = await _grupoPersist.GetGruposTurmaIdAsync(turmaId)
                ?? throw new NotFoundException("Nenhum grupo encontrado");
            return _mapper.Map<IEnumerable<GrupoDTO>>(grupos);
        }
        catch
        {
            throw;
        }
    }
    public async Task<IEnumerable<AlunoGrupoCheckboxDTO>> GetAlunoGrupoTurma(int turmaId, int grupoId)
    {
        try
        {
            var alunos = await _alunoTurmaPersist.GetAlunosTurmaIdAsync(turmaId);
            var alunoGrupos = await _alunoGrupoPersist.GetAlunosGrupoTurmaId(turmaId);
            var alunosCheckbox = alunos.Select(aluno =>
            {
                var relacao = alunoGrupos
                    .FirstOrDefault(ag => ag.AlunoId == aluno.Id);
                return new AlunoGrupoCheckboxDTO
                {
                    AlunoId = aluno.Id,
                    Nome = aluno.Nome,

                    Selecionado = relacao?.GrupoId == grupoId,
                    Desabilitado = relacao != null && relacao.GrupoId != grupoId,
                    GrupoAtualId = relacao?.GrupoId
                };
            }).ToList();
            return alunosCheckbox;
        }
        catch
        {
            throw;
        }
    }
    #endregion
    #region add
    public async Task<GrupoDTO> Add(GrupoDTO model)
    {
        try
        {
            var turma = await _turmaPersist.GetTurmaIdAsync(model.TurmaId)
                ?? throw new NotFoundException("Turma não encontrada");

            var grupo = new Grupo(nome: model.Nome, turma: turma);

            _geralPersist.Add(grupo);
            _ = await _geralPersist.SaveChangesAsync();
            var grupoRetorno = await _grupoPersist.GetGrupoIdAsync(grupo.Id);
            return _mapper.Map<GrupoDTO>(grupoRetorno);
        }
        catch
        {
            throw;
        }
    }
    #endregion
    #region update
    public async Task<GrupoDTO> Update(int grupoId, GrupoDTO model)
    {
        try
        {
            var grupo = await _grupoPersist.GetGrupoIdAsync(grupoId)
                ?? throw new NotFoundException("Grupo não encontrado");
            grupo.AtualizarGrupo(model.Nome);
            _ = await _geralPersist.SaveChangesAsync();
            var grupoRetorno = await _grupoPersist.GetGrupoIdAsync(grupoId);
            return _mapper.Map<GrupoDTO>(grupoRetorno);
        }
        catch
        {
            throw;
        }
    }
    public async Task AtualizarGrupo(int turmaId, int grupoId, List<int> alunosSelecionados)
    {
        AlunoGrupo[] alunos = await _alunoGrupoPersist.GetAlunosGrupoTurmaId(turmaId);
        var remover = alunos.Where(ag => ag.GrupoId == grupoId);

        _geralPersist.DeleteRange(remover);

        var novos = alunosSelecionados.Select(alunoId =>
            new AlunoGrupo
            (
                alunoId,
                grupoId,
                turmaId
            ));

        _geralPersist.AddRangeAsync(novos);
        _ = await _geralPersist.SaveChangesAsync();
    }
    #endregion
}
