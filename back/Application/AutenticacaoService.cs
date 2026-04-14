using AutoMapper;

using Application.Contracts;
using Application.DTOs;
using Application.Helpers;
using Domain.Entities;
using Persistence.Contracts;
using Domain.Exceptions;

namespace Application.Services;

public class AutenticacaoService(IGeralPersist geralPersist,
                    IProfessorPersist professorPersist,
                    IMapper mapper) : IAutenticacaoService
{
    private readonly IGeralPersist _geralPersist = geralPersist;
    private readonly IProfessorPersist _professorPersist = professorPersist;
    private readonly IMapper _mapper = mapper;

    #region get
    public async Task<string> Login(string userName, string senha)
    {
        try
        {
            var professor = await _professorPersist.GetProfessorUser(userName);

            if (professor == null || !BCrypt.Net.BCrypt.Verify(senha, professor.SenhaHash))
                throw new UnauthorizedException("Acesso inválido");

            var token = JWTToken.GenerateJWTToken();

            return token;
        }
        catch
        {
            throw;
        }
    }
    #endregion
    #region add
    public async Task<ProfessorDTO> Add(ProfessorDTO model)
    {
        try
        {
            var existe = _professorPersist.GetProfessorUser(model.UserName);
            if (existe != null)
                throw new BusinessException("Usuário já existe");

            var senhaHash = BCrypt.Net.BCrypt.HashPassword(model.SenhaHash);

            var professor = new Professor(model.UserName, senhaHash, model.Nome);

            _geralPersist.Add(professor);
            _ = await _geralPersist.SaveChangesAsync();

            return _mapper.Map<ProfessorDTO>(professor);
        }
        catch
        {
            throw;
        }
    }
    #endregion
    #region update
    #endregion
}