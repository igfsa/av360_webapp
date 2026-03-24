using AutoMapper;

using Application.Contracts;
using Application.DTOs;
using Domain.Entities;
using Persistence.Contracts;
using Domain.Exceptions;

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
            var NotaFinal = await _NotaFinalPersist.GetNotaFinalIdAsync(id)
                ?? throw new NotFoundException("Nota Final não encontrada");
            return _mapper.Map<NotaFinalDTO>(NotaFinal);
        }
        catch {
            throw;
    }}
    public async Task<NotaFinalDTO> GetNotasFinaisAlunoSessao(int alunoId, int sessaoId) {
        try {
            var NotasFinais = await _NotaFinalPersist.GetNotaFinalAlunoSessaoIdAsync(alunoId, sessaoId)
                ?? throw new NotFoundException("Notas Finais não encontradas");
            return _mapper.Map<NotaFinalDTO>(NotasFinais);
        }
        catch {
            throw;
    }}
    public async Task<IEnumerable<NotaFinalDTO>> GetNotasFinaisGrupoSessao(int grupoId, int sessaoId) {
        try {
            var NotasFinais = await _NotaFinalPersist.GetNotaFinalGrupoSessaoIdAsync(grupoId, sessaoId)
                ?? throw new NotFoundException("Notas Finais não encontradas");
            return _mapper.Map<IEnumerable<NotaFinalDTO>>(NotasFinais);
        }
        catch {
            throw;
    }}
    #endregion
    #region add
    public async Task<NotaFinalDTO> Add(NotaFinalDTO model) {
        try {
            var NotaFinal = _mapper.Map<NotaFinal>(model);
            _geralPersist.Add(NotaFinal);
            await _geralPersist.SaveChangesAsync();
            var NotaFinalRetorno = await _NotaFinalPersist.GetNotaFinalIdAsync(NotaFinal.Id);
            return _mapper.Map<NotaFinalDTO>(NotaFinalRetorno);
        }
        catch {
            throw;
    }}
    #endregion
    #region update
    public async Task<NotaFinalDTO> Update(int NotaFinalId, NotaFinalDTO model) {
        try {
            var NotaFinal = await _NotaFinalPersist.GetNotaFinalIdAsync(NotaFinalId)
                ?? throw new NotFoundException("Nota Final não encontrada");

            model.Id = NotaFinal.Id;
            _mapper.Map(model, NotaFinal);
            _geralPersist.Update(NotaFinal);

            var NotaFinalRetorno = await _NotaFinalPersist.GetNotaFinalIdAsync(NotaFinalId);
            return _mapper.Map<NotaFinalDTO>(NotaFinalRetorno);
        }
        catch {
            throw;
    }}
    #endregion
}
