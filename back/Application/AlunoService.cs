using AutoMapper;

using Application.Contracts;
using Application.DTOs;
using Domain.Entities;
using Persistence.Contracts;
using Application.Helpers;
using Domain.Exceptions;

namespace Application.Services;

public class AlunoService(IGeralPersist geralPersist,
                    IAlunoPersist alunoPersist,
                    IAlunoTurmaPersist alunoTurmaPersist,
                    IAlunoGrupoPersist alunoGrupoPersist,
                    IGrupoPersist grupoPersist,
                    ITurmaPersist turmaPersist,
                    IMapper mapper) : IAlunoService
{
    private readonly IGeralPersist _geralPersist = geralPersist;
    private readonly IAlunoPersist _alunoPersist = alunoPersist;
    private readonly IAlunoTurmaPersist _alunoTurmaPersist = alunoTurmaPersist;
    private readonly IAlunoGrupoPersist _alunoGrupoPersist = alunoGrupoPersist;
    private readonly IGrupoPersist _grupoPersist = grupoPersist;
    private readonly ITurmaPersist _turmaPersist = turmaPersist;
    private readonly IMapper _mapper = mapper;

    #region get
    public async Task<IEnumerable<AlunoDTO>> GetAlunos()
    {
        try
        {
            var alunos = await _alunoPersist.GetAllAlunosAsync()
                ?? throw new NotFoundException("Nenhum aluno encontrado");
            return _mapper.Map<IEnumerable<AlunoDTO>>(alunos);
        }
        catch
        {
            throw ; 
        }
    }

    public async Task<AlunoDTO> GetAlunoById(int Id)
    {
        try
        {
            var aluno = await _alunoPersist.GetAlunoIdAsync(Id)
                ?? throw new NotFoundException("Aluno não encontrado");
            return _mapper.Map<AlunoDTO>(aluno);
        }
        catch
        {
            throw;
        }
    }

    public async Task<AlunoDTO> GetAlunoByNomeIdGrupo(string nome, int grupoId)
    {
        try
        {
            IEnumerable<AlunoDTO>? alunos = await GetAlunosGrupo(grupoId)
                ?? throw new NotFoundException("Nenhum aluno encontrado");
            AlunoDTO? aluno = alunos.FirstOrDefault(a =>
            Texto.Normalizar(a.Nome) == Texto.Normalizar(nome))
                ?? throw new NotFoundException("Aluno não encontrado");
            return _mapper.Map<AlunoDTO>(aluno);
        }
        catch
        {
            throw;
        }
    }

    public async Task<IEnumerable<AlunoDTO>> GetAlunosTurma(int turmaId)
    {
        try
        {
            var alunos = await _alunoTurmaPersist.GetAlunosTurmaIdAsync(turmaId)
                ?? throw new NotFoundException("Nenhum aluno encontrado");

            return _mapper.Map<IEnumerable<AlunoDTO>>(alunos);
        }
        catch
        {
            throw;
        }
    }
    public async Task<IEnumerable<AlunoDTO>> GetAlunosGrupo(int grupoId)
    {
        try
        {
            var alunos = await _alunoGrupoPersist.GetAlunosGrupoId(grupoId)
                ?? throw new NotFoundException("Nenhum aluno encontrado");

            return _mapper.Map<IEnumerable<AlunoDTO>>(alunos);
        }
        catch
        {
            throw;
        }
    }
    public async Task<IEnumerable<AlunoGrupoNomeDTO>> GetAlunoGrupoNome(int turmaId)
    {
        try
        {
            var alunos = await _alunoTurmaPersist.GetAlunosTurmaIdAsync(turmaId);
            var alunoGrupos = await _alunoGrupoPersist.GetAlunosGrupoTurmaId(turmaId);
            var grupos = await _grupoPersist.GetGruposTurmaIdAsync(turmaId);
            var turma = await _turmaPersist.GetTurmaIdAsync(turmaId)
                ?? throw new NotFoundException("Turma não encontrada");

            var alunosGrupoNome = alunos.Select(aluno =>
            {
                var relacao = alunoGrupos.FirstOrDefault(ag => ag.AlunoId == aluno.Id);

                var grupo = relacao != null ? grupos.FirstOrDefault(g => g.Id == relacao.GrupoId) : null;
                return new AlunoGrupoNomeDTO
                {
                    AlunoId = aluno.Id,
                    Nome = aluno.Nome,

                    GrupoId = grupo?.Id,
                    GrupoNome = grupo?.Nome,

                    TurmaId = turma.Id,
                    TurmaCod = turma.Cod
                };
            }).ToList();
            return alunosGrupoNome;
        }
        catch
        {
            throw;
        }
    }
    #endregion
    #region add
    public async Task<AlunoDTO> Add(AlunoDTO model)
    {
        try
        {
            var aluno = new Aluno(model.Nome);

            _geralPersist.Add(aluno);

            _ = await _geralPersist.SaveChangesAsync();
            return _mapper.Map<AlunoDTO>(aluno);
        }
        catch
        {
            throw;
        }
    }
    #endregion
    #region update
    public async Task<AlunoDTO> Update(int alunoId, AlunoDTO model)
    {
        try
        {
            var aluno = await _alunoPersist.GetAlunoIdAsync(alunoId)
                ?? throw new NotFoundException("Aluno não encontrado");

            model.Id = aluno.Id;
            _ = _mapper.Map(model, aluno);
            aluno.AtualizarAluno(aluno.Nome);
            _ = await _geralPersist.SaveChangesAsync();
            var alunoRetorno = await _alunoPersist.GetAlunoIdAsync(alunoId);
            return _mapper.Map<AlunoDTO>(alunoRetorno);
        }
        catch
        {
            throw;
        }
    }
    #endregion
}
