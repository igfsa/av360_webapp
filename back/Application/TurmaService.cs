using AutoMapper;

using Application.Contracts;
using Application.DTOs;
using Domain.Entities;
using Persistence.Contracts;

namespace Application.Services;

public class TurmaService : ITurmaService
{
    private readonly IGeralPersist _geralPersist;
    private ITurmaPersist _turmaPersist;
    private IAlunoPersist _alunoPersist;
    private IAlunoTurmaPersist _alunoTurmaPersist;
    private readonly IMapper _mapper;

    public TurmaService(IGeralPersist geralPersist,
                        ITurmaPersist TurmaPersist, 
                        IAlunoTurmaPersist alunoTurmaPersist,
                        IAlunoPersist alunoPersist,
                        IMapper mapper)
    {
        _geralPersist = geralPersist;
        _turmaPersist = TurmaPersist;
        _alunoPersist = alunoPersist;
        _alunoTurmaPersist = alunoTurmaPersist;
        _mapper = mapper;
    }

    #region get
    public async Task<IEnumerable<TurmaDTO>> GetTurmas()
    {
        try
        {
            var turmas= await _turmaPersist.GetAllTurmasAsync();
            if (turmas== null) return null;

            var resultado = _mapper.Map<IEnumerable<TurmaDTO>>(turmas);

            return resultado;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<TurmaDTO> GetTurmaById(int Id)
    {
        try
        {
            var Turma = await _turmaPersist.GetTurmaIdAsync(Id);
            if (Turma == null) return null;

            var resultado = _mapper.Map<TurmaDTO>(Turma);

            return resultado;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<IEnumerable<TurmaDTO>> GetTurmasAluno(int turmaId)
    {
        try
        {
            var alunos = await _alunoTurmaPersist.GetTurmasAlunoIdAsync(turmaId);
            if (alunos == null) return null;

            var resultado = _mapper.Map<IEnumerable<TurmaDTO>>(alunos);

            return resultado;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }    
    #endregion
    #region add
    public async Task<TurmaDTO> Add(TurmaDTO model)
    {
        try
        {
            var Turma = _mapper.Map<Turma>(model);

            _geralPersist.Add(Turma);

            if (await _geralPersist.SaveChangesAsync())
            {
                var TurmaRetorno = await _turmaPersist.GetTurmaIdAsync(Turma.Id);

                return _mapper.Map<TurmaDTO>(TurmaRetorno);
            }
            return null;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }    
    public async Task<AlunoDTO> AddTurmaAluno(int alunoId, int turmaId)
    {
        try
        {
            // Aluno aluno = await _alunoTurmaPersist.GetValidaAlunoTurma(alunoId, turmaId);
            // return _mapper.Map<AlunoDTO>(aluno);
            Aluno aluno = await _alunoTurmaPersist.GetValidaAlunoTurma(turmaId, alunoId);
            Turma turma = await _turmaPersist.GetTurmaIdAsync(turmaId);
            if( aluno != null && turma != null)
            {
                AlunoTurma at = new()
                {
                    TurmaId = turmaId,
                    AlunoId = alunoId,
                };

                _geralPersist.Add(at);
                                
                if (await _geralPersist.SaveChangesAsync())
                {
                    return _mapper.Map<AlunoDTO>(aluno);
                }
                throw new Exception("Erro");
            }
            else
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    #endregion
    #region update
    public async Task<TurmaDTO> Update(int TurmaId, TurmaDTO model)
    {
        try
        {
            var Turma = await _turmaPersist.GetTurmaIdAsync(TurmaId);
            if (Turma == null) return null;

            model.Id = Turma.Id;

            _mapper.Map(model, Turma);

            _geralPersist.Update(Turma);

            if (await _geralPersist.SaveChangesAsync())
            {
                var TurmaRetorno = await _turmaPersist.GetTurmaIdAsync(TurmaId);

                return _mapper.Map<TurmaDTO>(TurmaRetorno);
            }
            return null;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    #endregion
}
