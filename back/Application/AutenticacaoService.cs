using AutoMapper;

using Application.Contracts;
using Application.DTOs;
using Application.Helpers;
using Domain.Entities;
using Persistence.Contracts;
using Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class AutenticacaoService(IGeralPersist geralPersist,
                    IProfessorPersist professorPersist,
                    IRefreshTokenPersist refreshTokenPersist,
                    IMapper mapper,
                    JWTToken jwtToken,
                    ILogger<AutenticacaoService> logger) : IAutenticacaoService
{
    private readonly IGeralPersist _geralPersist = geralPersist;
    private readonly IProfessorPersist _professorPersist = professorPersist;
    private readonly IRefreshTokenPersist _refreshTokenPersist = refreshTokenPersist;
    private readonly IMapper _mapper = mapper;
    private readonly JWTToken _jwtToken = jwtToken;

    private readonly ILogger _logger = logger;

    #region get
    public async Task Login(string userName, string senha, HttpResponse response)
    {
        try
        {
            var professor = await _professorPersist.GetProfessorUser(userName);

            if (professor == null || !BCrypt.Net.BCrypt.Verify(senha, professor.SenhaHash))
                throw new UnauthorizedException("Acesso inválido");

            var refreshToken = new RefreshToken
            (
                professor
            );

            _geralPersist.Add(refreshToken);

            var novoAccessToken = _jwtToken.GenerateJWTToken();

            await _geralPersist.SaveChangesAsync();

            CookiesHelper.SetCookies(response, novoAccessToken, refreshToken.Token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro de login: {userName}", userName);
            throw;
        }
    }
    #endregion
    #region add
    public async Task<ProfessorDTO> Add(ProfessorDTO model)
    {
        try
        {
            var existe = await _professorPersist.GetProfessorUser(model.UserName);
            if (existe != null)
                throw new BusinessException("Usuário já existe");

            var senhaHash = BCrypt.Net.BCrypt.HashPassword(model.SenhaHash);

            var professor = new Professor(model.UserName, senhaHash, model.Nome);

            _geralPersist.Add(professor);
            _ = await _geralPersist.SaveChangesAsync();

            return _mapper.Map<ProfessorDTO>(professor);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar Professor: {model.Nome}", model.Nome);
            throw;
        }
    }
    #endregion
    #region update
    public async Task Refresh(HttpRequest request, HttpResponse response)
    {
        try
        {
            var token = request.Cookies["refresh_token"]
                ?? throw new UnauthorizedException("Cookie Refresh inválido");

            var refreshToken = await _refreshTokenPersist.GetRefreshToken(token);

            if (refreshToken == null || !refreshToken.Validar())
                throw new UnauthorizedException("Token inválido ou expirado");

            refreshToken.Revogar();

            var novoRefresh = new RefreshToken
            (
                refreshToken.Professor
            );

            _geralPersist.Add(novoRefresh);

            var novoAccessToken = _jwtToken.GenerateJWTToken();

            await _geralPersist.SaveChangesAsync();

            CookiesHelper.SetCookies(response, novoAccessToken, novoRefresh.Token);
        }
        catch
        {
            throw;
        }
    }

    public async Task Logout(HttpRequest request, HttpResponse response)
    {
        try
        {
            var token = request.Cookies["refresh_token"] ??
                throw new ValidationException("Cookie Inválido");

            var refreshToken = await _refreshTokenPersist.GetRefreshToken(token);

            if (refreshToken != null)
            {
                refreshToken.Revogar();

                await _geralPersist.SaveChangesAsync();
            }

            response.Cookies.Delete("auth_token");
            response.Cookies.Delete("refresh_token");
        }
        catch
        {
            throw;
        }
    }
    #endregion
}