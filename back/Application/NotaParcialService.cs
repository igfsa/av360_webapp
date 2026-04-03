using AutoMapper;

using Application.Contracts;
using Application.DTOs;
using Domain.Entities;
using Persistence.Contracts;
using Domain.Exceptions;

namespace Application.Services;

public class NotaParcialService(IGeralPersist geralPersist,
                    INotaParcialPersist NotaParcialPersist,
                    IMapper mapper) : INotaParcialService
{
    private readonly IGeralPersist _geralPersist = geralPersist;
    private readonly INotaParcialPersist _NotaParcialPersist = NotaParcialPersist;
    private readonly IMapper _mapper = mapper;

    #region get

    public async Task<NotaParcialDTO> GetById(int id)
    {
        try
        {
            var NotaParcial = await _NotaParcialPersist.GetById(id)
                ?? throw new NotFoundException("Parcial não encontrada");
            return _mapper.Map<NotaParcialDTO>(NotaParcial);
        }
        catch
        {
            throw;
        }
    }
    public async Task<IEnumerable<NotaParcialDTO>> GetNotaParcialNFinalId(int notaFinalId)
    {
        try
        {
            var NotasParciais = await _NotaParcialPersist.GetNotaParcialNFinalIdAsync(notaFinalId)
                ?? throw new NotFoundException("Parciais não encontradas");
            return _mapper.Map<IEnumerable<NotaParcialDTO>>(NotasParciais); ;
        }
        catch
        {
            throw;
        }
    }
    #endregion
    #region add
    public async Task<NotaParcialDTO> Add(NotaParcialDTO model)
    {
        try
        {
            var NotaParcial = _mapper.Map<NotaParcial>(model);
            _geralPersist.Add(NotaParcial);
            _ = await _geralPersist.SaveChangesAsync();
            var NotaParcialRetorno = await _NotaParcialPersist.GetById(NotaParcial.Id);
            return _mapper.Map<NotaParcialDTO>(NotaParcialRetorno);
        }
        catch
        {
            throw;
        }
    }
    #endregion
    #region update
    public async Task<NotaParcialDTO> Update(int NotaParcialId, NotaParcialDTO model)
    {
        try
        {
            var NotaParcial = await _NotaParcialPersist.GetById(NotaParcialId)
                ?? throw new NotFoundException("Parcial não encontrada");
            model.Id = NotaParcial.Id;

            _ = _mapper.Map(model, NotaParcial);

            _geralPersist.Update(NotaParcial);

            _ = await _geralPersist.SaveChangesAsync();
            var NotaParcialRetorno = await _NotaParcialPersist.GetById(NotaParcialId);
            return _mapper.Map<NotaParcialDTO>(NotaParcialRetorno);
        }
        catch
        {
            throw;
        }
    }
    #endregion
}
