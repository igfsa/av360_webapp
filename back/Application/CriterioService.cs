using AutoMapper;

using Application.Contracts;
using Application.DTOs;
using Domain.Entities;
using Persistence.Contracts;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;
using Application.Helpers;

namespace Application.Services;

public class CriterioService(IGeralPersist geralPersist,
                    ICriterioPersist criterioPersist,
                    IProfessorPersist professorPersist,
                    IMapper mapper,
                    ICriterioTurmaPersist criterioTurmaPersist,
                    ICurrentUser user,
                    ILogger<CriterioService> logger) : ICriterioService
{
    private readonly IGeralPersist _geralPersist = geralPersist;
    private readonly ICriterioPersist _criterioPersist = criterioPersist;
    private readonly IProfessorPersist _professorPersist = professorPersist;
    private readonly ICriterioTurmaPersist _criterioTurmaPersist = criterioTurmaPersist;
    private readonly ICurrentUser _user = user;

    private readonly IMapper _mapper = mapper;
    private readonly ILogger _logger = logger;

    #region get
    public async Task<IEnumerable<CriterioDTO>> GetCriterios()
    {
        try
        {
            var criterios = await _criterioPersist.GetAllCriteriosAsync()
                ?? throw new NotFoundException("Nenhum critério encontrado");

            return _mapper.Map<IEnumerable<CriterioDTO>>(criterios);
        }
        catch
        {
            throw;
        }
    }
    public async Task<IEnumerable<CriterioDTO>> GetCriteriosProfessor()
    {
        try
        {
            var criterios = await _criterioPersist.GetCriteriosProfessorAsync(_user.ProfessorId)
                ?? throw new NotFoundException("Nenhum critério encontrado");

            return _mapper.Map<IEnumerable<CriterioDTO>>(criterios);
        }
        catch
        {
            throw;
        }
    }
    public async Task<IEnumerable<CriterioDTO>> GetCriteriosTurma(int turmaId)
    {
        try
        {
            var criterios = await _criterioTurmaPersist.GetCriteriosTurmaIdAsync(turmaId)
                ?? throw new NotFoundException("Nenhum critério encontrado");

            return _mapper.Map<IEnumerable<CriterioDTO>>(criterios);
        }
        catch
        {
            throw;
        }
    }
    public async Task<IEnumerable<string>> GetCriteriosSugestoes()
    {
        var todos = await _criterioPersist.GetCriteriosSugestaoAsync(_user.ProfessorId);

        var minhasChaves = (await _criterioPersist.GetCriteriosProfessorAsync(_user.ProfessorId))
            .Select(c => Texto.ChaveComparacao(c.Nome))
            .ToHashSet();

        return [.. todos
            .GroupBy(c => Texto.ChaveComparacao(c.Nome))
            .Where(g => !minhasChaves.Contains(g.Key))
            .Select(g => g
                .GroupBy(c => c.Nome)
                .OrderByDescending(x => x.Count())
                .First().Key)
            .OrderBy(n => n, StringComparer.CurrentCultureIgnoreCase)];
    }
    #endregion
    #region add
    public async Task<CriterioDTO> Add(CriterioDTO model)
    {
        try
        {
            var professor = await _professorPersist.GetProfessorIdAsync(_user.ProfessorId)
                ?? throw new NotFoundException("Professor não encontrado");
            var criterio = new Criterio(
                nome: model.Nome,
                professor: professor
            );
            _geralPersist.Add(criterio);

            _ = await _geralPersist.SaveChangesAsync();
            var CriterioRetorno = await _criterioPersist.GetCriterioIdAsync(criterio.Id);
            return _mapper.Map<CriterioDTO>(CriterioRetorno);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar Critério: {model.Nome}", model.Nome);
            throw;
        }
    }
    #endregion
    #region update
    public async Task<CriterioDTO> Update(int criterioId, CriterioDTO model)
    {
        try
        {
            var criterio = await _criterioPersist.GetCriterioIdAsync(criterioId)
                ?? throw new NotFoundException("Critério não encontrado");
            criterio.AtualizarCriterio(model.Nome);

            _ = await _geralPersist.SaveChangesAsync();
            var criterioRetorno = await _criterioPersist.GetCriterioIdAsync(criterioId);
            return _mapper.Map<CriterioDTO>(criterioRetorno);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar Critério: {criterioId}", criterioId);
            throw;
        }
    }
    #endregion 
}
