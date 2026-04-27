using AutoMapper;

using Application.Contracts;
using Application.DTOs;
using Domain.Entities;
using Persistence.Contracts;
using Domain.Exceptions;

namespace Application.Services;

public class CriterioService(IGeralPersist geralPersist,
                    ICriterioPersist criterioPersist,
                    IMapper mapper,
                    ICriterioTurmaPersist criterioTurmaPersist) : ICriterioService
{
    private readonly IGeralPersist _geralPersist = geralPersist;
    private readonly ICriterioPersist _criterioPersist = criterioPersist;
    private readonly ICriterioTurmaPersist _criterioTurmaPersist = criterioTurmaPersist;

    private readonly IMapper _mapper = mapper;

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
    #endregion
    #region add
    public async Task<CriterioDTO> Add(CriterioDTO model)
    {
        try
        {
            var criterio = new Criterio(
                nome: model.Nome
            );
            _geralPersist.Add(criterio);

            _ = await _geralPersist.SaveChangesAsync();
            var CriterioRetorno = await _criterioPersist.GetCriterioIdAsync(criterio.Id);
            return _mapper.Map<CriterioDTO>(CriterioRetorno);
        }
        catch
        {
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
        catch
        {
            throw;
        }
    }
    #endregion 
}
