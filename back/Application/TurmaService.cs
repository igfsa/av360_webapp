using System.Globalization;
using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;

using Application.Contracts;
using Application.DTOs;
using Application.Helpers;
using Domain.Entities;
using Persistence.Contracts;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class TurmaService(IGeralPersist geralPersist,
                    ITurmaPersist turmaPersist,
                    ICriterioPersist criterioPersist,
                    IProfessorPersist professorPersist,
                    IMapper mapper,
                    ILogger<TurmaService> logger) : ITurmaService
{
    private readonly IGeralPersist _geralPersist = geralPersist;
    private readonly ITurmaPersist _turmaPersist = turmaPersist;
    private readonly ICriterioPersist _criterioPersist = criterioPersist;
    private readonly IProfessorPersist _professorPersist = professorPersist;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger _logger = logger;

    #region get
    public async Task<IEnumerable<TurmaAdmDTO>> GetTurmasAdm()
    {
        try
        {
            var turmas = await _turmaPersist.GetAllTurmasAsync()
                ?? throw new NotFoundException("Nenhuma turma encontrada");

            List<TurmaAdmDTO> turmasAdm;

            turmasAdm = [.. turmas
                .Select(t => {
                    return new TurmaAdmDTO
                    {
                        Id = t.Id,
                        Cod = t.Cod,
                        NotaMax = t.NotaMax,
                        ProfessorId = t.ProfessorId,
                        ProfessorNome = t.Professor?.Nome
                    };
                })
                .OrderBy(t => t.Cod)];
            return turmasAdm;
        }
        catch
        {
            throw;
        }
    }
    public async Task<TurmaDTO> GetTurmaById(int Id)
    {
        try
        {
            var turma = await _turmaPersist.GetTurmaIdAsync(Id)
                ?? throw new NotFoundException("Turma não encontrada");
            return _mapper.Map<TurmaDTO>(turma);
        }
        catch
        {
            throw;
        }
    }
    public async Task<IEnumerable<TurmaDTO>> GetTurmasProfessorId(int professorId)
    {
        try
        {
            var turmas = await _turmaPersist.GetTurmasProfessorIdAsync(professorId)
                ?? throw new NotFoundException("Nenhum turma encontrado");
            return _mapper.Map<IEnumerable<TurmaDTO>>(turmas);
        }
        catch
        {
            throw;
        }
    }
    #endregion
    #region add
    public async Task<TurmaDTO> Add(TurmaDTO model)
    {
        try
        {
            var professor = await _professorPersist.GetProfessorIdAsync(model.ProfessorId ?? 0)
                ?? throw new NotFoundException("Professor não encontrado");

            var turma = new Turma(cod: model.Cod, notaMax: model.NotaMax, professor: professor);
            _geralPersist.Add(turma);
            await _geralPersist.SaveChangesAsync();

            var criteriosPadrao = await _criterioPersist.GetCriteriosPadrao();
            turma.AtualizarCriterios(criteriosPadrao);
            await _geralPersist.SaveChangesAsync();

            return _mapper.Map<TurmaDTO>(turma);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar turma: {model.nome}", model.Cod);
            throw;
        }
    }
    public async Task<TurmaDTO> AddTurmaCriterio(TurmaCriterioDTO model)
    {
        try
        {
            var turma = await _turmaPersist.GetTurmaIdAsync(model.TurmaId)
                ?? throw new NotFoundException("Turma não encontrada");
            var criterios = await _criterioPersist.GetAllCriteriosAsync();
            var criteriosTurma = criterios.Where(c => model.CriterioIds.Contains(c.Id));
            turma.AtualizarCriterios(criteriosTurma);
            await _geralPersist.SaveChangesAsync();
            return _mapper.Map<TurmaDTO>(turma);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao adicionar critério: {model.CriterioIds.ToString()} turma: {model.TurmaId}", model.CriterioIds.ToString(), model.TurmaId);
            throw;
        }
    }
    public async Task<CsvImportResultDTO> ImportarAlunosAsync(int turmaId, CsvImportRequestDTO dto)
    {
        try{
            var resultado = new CsvImportResultDTO();
            var delimiter = CSV.DetectarDelimitador(dto.Arquivo.OpenReadStream());

            var config = new CsvConfiguration(new CultureInfo("pt-BR"))
            {
                HasHeaderRecord = true,
                Delimiter = delimiter,
                TrimOptions = TrimOptions.Trim,
                IgnoreBlankLines = true,
                MissingFieldFound = null,
                BadDataFound = null,
                HeaderValidated = null
            };
            using var stream = dto.Arquivo.OpenReadStream();
            var encoding = Texto.EncodingDetector(stream);
            using var reader = new StreamReader(stream, encoding, detectEncodingFromByteOrderMarks: true);
            using var csv = new CsvReader(reader, config);
            await csv.ReadAsync();
            csv.ReadHeader();
            var headers = csv.HeaderRecord
                ?? throw new Exception($"Colunas da tabela não encontradas.");
            foreach (var h in headers)
            {
                Console.WriteLine($"HEADER: '{h}'");
            }
            var colunaNome = headers.FirstOrDefault(h => Texto.Normalizar(h) == Texto.Normalizar(dto.ColunaNome))
                ?? throw new BusinessException($"Coluna '{dto.ColunaNome}' não encontrada.");
            Turma turma = await _turmaPersist.GetTurmaIdAsync(turmaId)
                ?? throw new NotFoundException("Turma não encontrada");
            int linha = 1;
            while (await csv.ReadAsync())
            {
                linha++;
                resultado.Total++;
                var nome = csv.GetField(colunaNome);
                try
                {
                    if (string.IsNullOrWhiteSpace(nome))
                        throw new Domain.Exceptions.ValidationException("Nome vazio");
                    var aluno = new Aluno(nome);
                    _geralPersist.Add(aluno);
                    await _geralPersist.SaveChangesAsync();
                    turma.AdicionarAluno(aluno);
                    await _geralPersist.SaveChangesAsync();
                    resultado.Sucesso++;
                }
                catch (Exception ex)
                {
                    resultado.Falhas++;
                    resultado.Erros.Add(new CsvImportErrorDTO
                    {
                        Linha = linha,
                        Nome = nome,
                        Erro = ex.Message
                    });
                }
            }
            await _geralPersist.SaveChangesAsync();
            return resultado;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao adicionar alunos na turma: {turmaId}", turmaId);
            throw;
        }
    }
    #endregion
    #region update
    public async Task<TurmaDTO> Update(int turmaId, TurmaDTO model)
    {
        try
        {
            var turma = await _turmaPersist.GetTurmaIdAsync(turmaId)
                ?? throw new NotFoundException("Turma não encontrada");
            turma.AtualizarTurma(model.Cod, model.NotaMax);
            await _geralPersist.SaveChangesAsync();
            var TurmaRetorno = await _turmaPersist.GetTurmaIdAsync(turmaId);
            return _mapper.Map<TurmaDTO>(TurmaRetorno);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar turma: {turmaId}", turmaId);
            throw;
        }
    }
    public async Task<TurmaAdmDTO> AlterarProfessor(int turmaId, int professorId)
    {
        try
        {
            Console.WriteLine(turmaId);
            Console.WriteLine(professorId);
            var turma = await _turmaPersist.GetTurmaIdAsync(turmaId)
                ?? throw new NotFoundException("Turma não encontrada");
            var professor = await _professorPersist.GetProfessorIdAsync(professorId)
                ?? throw new NotFoundException("Professor não encontrado");
            turma.AtualizarProfessor(professor);
            await _geralPersist.SaveChangesAsync();
            var turmaRetorno = await _turmaPersist.GetTurmaIdAsync(turmaId)
                ?? throw new Exception();
            return new TurmaAdmDTO
                {
                    Id = turmaRetorno.Id,
                    Cod = turmaRetorno.Cod,
                    NotaMax = turmaRetorno.NotaMax,
                    ProfessorId = turmaRetorno.ProfessorId,
                    ProfessorNome = turmaRetorno.Professor?.Nome
                };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar turma: {turmaId}", turmaId);
            throw;
        }
    }

    public async Task<TurmaAdmDTO> RemoverProfessor(int turmaId)
    {
        try
        {
            var turma = await _turmaPersist.GetTurmaIdAsync(turmaId)
                ?? throw new NotFoundException("Turma não encontrada");
            turma.RemoverProfessor();
            await _geralPersist.SaveChangesAsync();
            var turmaRetorno = await _turmaPersist.GetTurmaIdAsync(turmaId)
                ?? throw new Exception();;
            return new TurmaAdmDTO
                {
                    Id = turmaRetorno.Id,
                    Cod = turmaRetorno.Cod,
                    NotaMax = turmaRetorno.NotaMax,
                    ProfessorId = turmaRetorno.ProfessorId,
                    ProfessorNome = turmaRetorno.Professor?.Nome
                };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar turma: {turmaId}", turmaId);
            throw;
        }
    }
    #endregion
}
