using AutoMapper;

using Application.Contracts;
using Application.DTOs;
using Domain.Entities;
using Persistence.Contracts;

namespace Application.Services;

public class CriterioService : ICriterioService
{
    private readonly IGeralPersist _geralPersist;
    private ICriterioPersist _criterioPersist;
    private readonly ITurmaService _turmaService;

    private readonly IMapper _mapper;

    public CriterioService(IGeralPersist geralPersist,
                        ICriterioPersist criterioPersist, 
                        ITurmaService turmaService,
                        IMapper mapper)
    {
        _geralPersist = geralPersist;
        _criterioPersist = criterioPersist;
        _turmaService = turmaService;
        _mapper = mapper;
    }

    #region get
    public async Task<IEnumerable<CriterioDTO>> GetCriterios()
    {
        try
        {
            var criterios = await _criterioPersist.GetAllCriteriosAsync();
            if (criterios == null) return null;

            var resultado = _mapper.Map<IEnumerable<CriterioDTO>>(criterios);

            return resultado;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<CriterioDTO> GetCriterioById(int Id)
    {
        try
        {
            var criterio = await _criterioPersist.GetCriterioIdAsync(Id);
            if (criterio == null) return null;

            var resultado = _mapper.Map<CriterioDTO>(criterio);

            return resultado;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<IEnumerable<CriterioDTO>> GetCriteriosTurma(int turmaId)
    {
        try
        {
            var criterios = await _criterioPersist.GetCriteriosTurmaAsync(turmaId);
            if (criterios == null) return null;

            var resultado = _mapper.Map<IEnumerable<CriterioDTO>>(criterios);

            return resultado;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    #endregion
    #region add
    public async Task<CriterioDTO> Add(CriterioDTO model)
    {
        try
        {
            var criterio = _mapper.Map<Criterio>(model);

            _geralPersist.Add(criterio);

            if (await _geralPersist.SaveChangesAsync())
            {
                var CriterioRetorno = await _criterioPersist.GetCriterioIdAsync(criterio.Id);

                return _mapper.Map<CriterioDTO>(CriterioRetorno);
            }
            return null;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    #endregion
    #region update
    public async Task<CriterioDTO> Update(int criterioId, CriterioDTO model)
    {
        try
        {
            var criterio = await _criterioPersist.GetCriterioIdAsync(criterioId);
            if (criterio == null) return null;

            model.Id = criterio.Id;

            _mapper.Map(model, criterio);

            _geralPersist.Update(criterio);

            if (await _geralPersist.SaveChangesAsync())
            {
                var criterioRetorno = await _criterioPersist.GetCriterioIdAsync(criterioId);

                return _mapper.Map<CriterioDTO>(criterioRetorno);
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
