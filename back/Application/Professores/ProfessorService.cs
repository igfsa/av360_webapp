using AutoMapper;

using Application.Contracts;
using Application.DTOs;
using Persistence.Contracts;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class ProfessorService(IGeralPersist geralPersist,
                    IProfessorPersist professorPersist,
                    IMapper mapper,
                    ILogger<ProfessorService> logger) : IProfessorService
{
    private readonly IGeralPersist _geralPersist = geralPersist;
    private readonly IProfessorPersist _professorPersist = professorPersist;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger _logger = logger;

    #region get
    public async Task<IEnumerable<ProfessorDTO>> GetProfessores()
    {
        try
        {
            var professors = await _professorPersist.GetAllProfessores()
                ?? throw new NotFoundException("Nenhum professor encontrado");

            return _mapper.Map<IEnumerable<ProfessorDTO>>(professors);
        }
        catch
        {
            throw;
        }
    }
    #endregion
    #region update
    public async Task<ProfessorDTO> Update(int professorId, ProfessorDTO model)
    {
        try
        {
            var professor = await _professorPersist.GetProfessorIdAsync(professorId)
                ?? throw new NotFoundException("Professor não encontrada");
            professor.AtualizarProfessor(model.Nome, model.Perfil);
            await _geralPersist.SaveChangesAsync();
            var ProfessorRetorno = await _professorPersist.GetProfessorIdAsync(professorId);
            return _mapper.Map<ProfessorDTO>(ProfessorRetorno);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar professor: {professorId}", professorId);
            throw;
        }
    }
    #endregion
}