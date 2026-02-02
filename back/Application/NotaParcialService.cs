using AutoMapper;

using Application.Contracts;
using Application.DTOs;
using Domain.Entities;
using Persistence.Contracts;
using System.Runtime.CompilerServices;

namespace Application.Services;

public class NotaParcialService : INotaParcialService
{
    private readonly IGeralPersist _geralPersist;
    private INotaParcialPersist _NotaParcialPersist;
    private readonly IMapper _mapper;

    public NotaParcialService(IGeralPersist geralPersist,
                        INotaParcialPersist NotaParcialPersist,
                        IMapper mapper)
    {
        _geralPersist = geralPersist;
        _NotaParcialPersist = NotaParcialPersist;
        _mapper = mapper;
    }

    #region get

    public async Task<NotaParcialDTO> GetById(int id) {
        try {
            var NotaParcial = await _NotaParcialPersist.GetById(id);
            if (NotaParcial == null) 
                return null;
            var resultado = _mapper.Map<NotaParcialDTO>(NotaParcial);
            return resultado;
        }
        catch (Exception ex) {
            throw new Exception(ex.Message);
    }}
    public async Task<IEnumerable<NotaParcialDTO>> GetNotaParcialNFinalId(int notaFinalId) {
        try {
            var NotasParciais = await _NotaParcialPersist.GetNotaParcialNFinalIdAsync(notaFinalId);
            if (NotasParciais == null) 
                return null;
            var resultado = _mapper.Map<IEnumerable<NotaParcialDTO>>(NotasParciais);
            return resultado;
        }
        catch (Exception ex) {
            throw new Exception(ex.Message);
    }}
    #endregion
    #region add
    public async Task<NotaParcialDTO> Add(NotaParcialDTO model) {
        try {
            var NotaParcial = _mapper.Map<NotaParcial>(model);
            _geralPersist.Add(NotaParcial);
            if (await _geralPersist.SaveChangesAsync()) {
                var NotaParcialRetorno = await _NotaParcialPersist.GetById(NotaParcial.Id);
                return _mapper.Map<NotaParcialDTO>(NotaParcialRetorno);
            }
            return null;
        }
        catch (Exception ex) {
            throw new Exception(ex.Message);
    }}
    #endregion
    #region update
    public async Task<NotaParcialDTO> Update(int NotaParcialId, NotaParcialDTO model) {
        try {
            var NotaParcial = await _NotaParcialPersist.GetById(NotaParcialId);
            if (NotaParcial == null) return null;

            model.Id = NotaParcial.Id;

            _mapper.Map(model, NotaParcial);

            _geralPersist.Update(NotaParcial);

            if (await _geralPersist.SaveChangesAsync()) {
                var NotaParcialRetorno = await _NotaParcialPersist.GetById(NotaParcialId);

                return _mapper.Map<NotaParcialDTO>(NotaParcialRetorno);
            }
            return null;
        }
        catch (Exception ex) {
            throw new Exception(ex.Message);
    }}
    #endregion
}
