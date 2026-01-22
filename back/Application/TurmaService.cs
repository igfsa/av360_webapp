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
    private IAlunoTurmaPersist _alunoTurmaPersist;
    private ICriterioTurmaPersist _criterioTurmaPersist;
    private readonly IMapper _mapper;

    public TurmaService(IGeralPersist geralPersist,
                        ITurmaPersist TurmaPersist, 
                        IAlunoTurmaPersist alunoTurmaPersist,
                        ICriterioTurmaPersist criterioTurmaPersist,
                        IMapper mapper)
    {
        _geralPersist = geralPersist;
        _turmaPersist = TurmaPersist;
        _alunoTurmaPersist = alunoTurmaPersist;
        _criterioTurmaPersist = criterioTurmaPersist;
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
    public async Task<IEnumerable<TurmaDTO>> GetTurmasAluno(int alunoId)
    {
        try
        {
            var turmas = await _alunoTurmaPersist.GetTurmasAlunoIdAsync(alunoId);
            if (turmas == null) return null;

            var resultado = _mapper.Map<IEnumerable<TurmaDTO>>(turmas);

            return resultado;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<IEnumerable<TurmaDTO>> GetTurmasCriterio(int criterioId)
    {
        try
        {
            var turmas = await _criterioTurmaPersist.GetTurmasCriterioIdAsync(criterioId);
            if (turmas == null) return null;

            var resultado = _mapper.Map<IEnumerable<TurmaDTO>>(turmas);

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
    public async Task<CriterioDTO> AddTurmaCriterio(int criterioId, int turmaId)
    {
        try
        {
            Criterio criterio = await _criterioTurmaPersist.GetValidaCriterioTurma(turmaId, criterioId);
            Turma turma = await _turmaPersist.GetTurmaIdAsync(turmaId);
            if( criterio != null && turma != null)
            {
                CriterioTurma ct = new()
                {
                    TurmaId = turmaId,
                    CriterioId = criterioId,
                };

                _geralPersist.Add(ct);
                                
                if (await _geralPersist.SaveChangesAsync())
                {
                    return _mapper.Map<CriterioDTO>(criterio);
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
    public async Task<TurmaDTO> Update(int turmaId, TurmaDTO model)
    {
        try
        {
            var turma = await _turmaPersist.GetTurmaIdAsync(turmaId);
            if (turma == null) return null;

            model.Id = turma.Id;

            _mapper.Map(model, turma);

            _geralPersist.Update(turma);

            if (await _geralPersist.SaveChangesAsync())
            {
                var TurmaRetorno = await _turmaPersist.GetTurmaIdAsync(turmaId);

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
