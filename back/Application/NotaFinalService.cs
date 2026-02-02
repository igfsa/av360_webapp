using AutoMapper;

using Application.Contracts;
using Application.DTOs;
using Domain.Entities;
using Persistence.Contracts;

namespace Application.Services;

public class NotaFinalService : INotaFinalService
{
    private readonly IGeralPersist _geralPersist;
    private INotaFinalPersist _NotaFinalPersist;
    private readonly IMapper _mapper;

    public NotaFinalService(IGeralPersist geralPersist,
                        INotaFinalPersist NotaFinalPersist,
                        IMapper mapper)
    {
        _geralPersist = geralPersist;
        _NotaFinalPersist = NotaFinalPersist;
        _mapper = mapper;
    }

    #region get

    public async Task<NotaFinalDTO> GetById(int id) {
        try {
            var NotaFinal = await _NotaFinalPersist.GetNotaFinalIdAsync(id);
            if (NotaFinal == null) 
                return null;
            var resultado = _mapper.Map<NotaFinalDTO>(NotaFinal);
            return resultado;
        }
        catch (Exception ex) {
            throw new Exception(ex.Message);
    }}
    public async Task<IEnumerable<NotaFinalDTO>> GetNotasFinaisAlunoSessao(int alunoId, int sessaoId) {
        try {
            var NotasFinais = await _NotaFinalPersist.GetNotaFinalAlunoSessaoIdAsync(alunoId, sessaoId);
            if (NotasFinais == null) 
                return null;
            var resultado = _mapper.Map<IEnumerable<NotaFinalDTO>>(NotasFinais);
            return resultado;
        }
        catch (Exception ex) {
            throw new Exception(ex.Message);
    }}
    public async Task<IEnumerable<NotaFinalDTO>> GetNotasFinaisGrupoSessao(int grupoId, int sessaoId) {
        try {
            var NotasFinais = await _NotaFinalPersist.GetNotaFinalGrupoSessaoIdAsync(grupoId, sessaoId);
            if (NotasFinais == null) 
                return null;
            var resultado = _mapper.Map<IEnumerable<NotaFinalDTO>>(NotasFinais);
            return resultado;
        }
        catch (Exception ex) {
            throw new Exception(ex.Message);
    }}
    #endregion
    #region add
    public async Task<NotaFinalDTO> Add(NotaFinalDTO model) {
        try {
            var NotaFinal = _mapper.Map<NotaFinal>(model);
            _geralPersist.Add(NotaFinal);
            if (await _geralPersist.SaveChangesAsync()) {
                var NotaFinalRetorno = await _NotaFinalPersist.GetNotaFinalIdAsync(NotaFinal.Id);
                return _mapper.Map<NotaFinalDTO>(NotaFinalRetorno);
            }
            return null;
        }
        catch (Exception ex) {
            throw new Exception(ex.Message);
    }}
    #endregion
    #region update
    public async Task<NotaFinalDTO> Update(int NotaFinalId, NotaFinalDTO model) {
        try {
            var NotaFinal = await _NotaFinalPersist.GetNotaFinalIdAsync(NotaFinalId);
            if (NotaFinal == null) return null;

            model.Id = NotaFinal.Id;

            _mapper.Map(model, NotaFinal);

            _geralPersist.Update(NotaFinal);

            if (await _geralPersist.SaveChangesAsync()) {
                var NotaFinalRetorno = await _NotaFinalPersist.GetNotaFinalIdAsync(NotaFinalId);

                return _mapper.Map<NotaFinalDTO>(NotaFinalRetorno);
            }
            return null;
        }
        catch (Exception ex) {
            throw new Exception(ex.Message);
    }}
    #endregion
}
