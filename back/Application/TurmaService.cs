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

namespace Application.Services;

public class TurmaService(IGeralPersist geralPersist,
                    ITurmaPersist turmaPersist,
                    ICriterioPersist criterioPersist,
                    IMapper mapper) : ITurmaService
{
    private readonly IGeralPersist _geralPersist = geralPersist;
    private readonly ITurmaPersist _turmaPersist = turmaPersist;
    private readonly ICriterioPersist _criterioPersist = criterioPersist;
    private readonly IMapper _mapper = mapper;

    #region get
    public async Task<IEnumerable<TurmaDTO>> GetTurmas()
    {
        try
        {
            var turmas = await _turmaPersist.GetAllTurmasAsync()
                ?? throw new NotFoundException("Nenhuma turma encontrada");
            return _mapper.Map<IEnumerable<TurmaDTO>>(turmas);
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
    #endregion
    #region add
    public async Task<TurmaDTO> Add(TurmaDTO model)
    {
        try
        {
            var turma = new Turma(cod: model.Cod, notaMax: model.NotaMax);
            _geralPersist.Add(turma);
            await _geralPersist.SaveChangesAsync();
            return _mapper.Map<TurmaDTO>(turma);
        }
        catch
        {
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
        catch
        {
            throw;
        }
    }
    public async Task<CsvImportResultDTO> ImportarAlunosAsync(int turmaId, CsvImportRequestDTO dto)
    {
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
        catch
        {
            throw;
        }
    }
    #endregion
}
