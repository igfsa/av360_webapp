using AutoMapper;

using Application.Contracts;
using Application.DTOs;
using Domain.Entities;
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
}