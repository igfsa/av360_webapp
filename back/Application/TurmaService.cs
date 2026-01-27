using System.Globalization;
using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;

using Application.Contracts;
using Application.DTOs;
using Application.Helpers;
using Domain.Entities;
using Persistence.Contracts;

namespace Application.Services;

public class TurmaService : ITurmaService
{
    private readonly IGeralPersist _geralPersist;
    private ITurmaPersist _turmaPersist;
    private IAlunoTurmaPersist _alunoTurmaPersist;
    private ICriterioTurmaPersist _criterioTurmaPersist;
    private readonly IMapper _mapper;

    public TurmaService(IGeralPersist geralPersist,
                        ITurmaPersist TurmaPersist,
                        IAlunoTurmaPersist alunoTurmaPersist,
                        ICriterioTurmaPersist criterioTurmaPersist,
                        IMapper mapper) {
        _geralPersist = geralPersist;
        _turmaPersist = TurmaPersist;
        _alunoTurmaPersist = alunoTurmaPersist;
        _criterioTurmaPersist = criterioTurmaPersist;
        _mapper = mapper;
    }

    #region get
    public async Task<IEnumerable<TurmaDTO>> GetTurmas(){
        try{
            var turmas= await _turmaPersist.GetAllTurmasAsync();
            if (turmas== null) return null;
            var resultado = _mapper.Map<IEnumerable<TurmaDTO>>(turmas);
            return resultado;
        }
        catch (Exception ex){
            throw new Exception(ex.Message);
    }}
    public async Task<TurmaDTO> GetTurmaById(int Id){
        try{
            var Turma = await _turmaPersist.GetTurmaIdAsync(Id);
            if (Turma == null) return null;
            var resultado = _mapper.Map<TurmaDTO>(Turma);
            return resultado;
        }
        catch (Exception ex){
            throw new Exception(ex.Message);
    }}
    public async Task<IEnumerable<TurmaDTO>> GetTurmasAluno(int alunoId){
        try{
            var turmas = await _alunoTurmaPersist.GetTurmasAlunoIdAsync(alunoId);
            if (turmas == null) 
                return null;
            var resultado = _mapper.Map<IEnumerable<TurmaDTO>>(turmas);
            return resultado;
        }
        catch (Exception ex){
            throw new Exception(ex.Message);
    }}
    public async Task<IEnumerable<TurmaDTO>> GetTurmasCriterio(int criterioId){
        try{
            var turmas = await _criterioTurmaPersist.GetTurmasCriterioIdAsync(criterioId);
            if (turmas == null) return null;
            var resultado = _mapper.Map<IEnumerable<TurmaDTO>>(turmas);
            return resultado;
        }
        catch (Exception ex){
            throw new Exception(ex.Message);
    }}
    #endregion
    #region add
    public async Task<TurmaDTO> Add(TurmaDTO model){
        try{
            var Turma = _mapper.Map<Turma>(model);
            _geralPersist.Add(Turma);
            if (await _geralPersist.SaveChangesAsync()){
                var TurmaRetorno = await _turmaPersist.GetTurmaIdAsync(Turma.Id);
                return _mapper.Map<TurmaDTO>(TurmaRetorno);
            }
            return null;
        }
        catch (Exception ex){
            throw new Exception(ex.Message);
    }}
    public async Task<AlunoDTO> AddTurmaAluno(int alunoId, int turmaId){
        try{
            Aluno aluno = await _alunoTurmaPersist.GetExisteAlunoTurma(turmaId, alunoId);
            Turma turma = await _turmaPersist.GetTurmaIdAsync(turmaId);
            if( aluno == null && turma != null){
                AlunoTurma at = new(){
                    TurmaId = turmaId,
                    AlunoId = alunoId,
                };
                _geralPersist.Add(at);
                if (await _geralPersist.SaveChangesAsync()){
                    return _mapper.Map<AlunoDTO>(aluno);
                }
                throw new Exception("Erro");
            }
            else{
                return null;
        }}
        catch (Exception ex){
            throw new Exception(ex.Message);
     }}    
    public async Task<TurmaDTO> AddTurmaCriterio(TurmaCriterioDTO model){
        try{
            var criteriosAtuais = await _criterioTurmaPersist.
                GetCriteriosByIdTurmaIdAsync(model.turmaId);
            var paraRemover = criteriosAtuais
                .Where(tc => !model.criterioIds.Contains(tc.CriterioId));
            _geralPersist.DeleteRange(paraRemover);
            var idsAtuais = criteriosAtuais.Select(tc => tc.CriterioId).ToHashSet();
            var paraAdicionar = model.criterioIds
                .Where(id => !idsAtuais.Contains(id))
                .Select(id => new CriterioTurma{
                    TurmaId = model.turmaId,
                    CriterioId = id
                });
            _geralPersist.AddRangeAsync(paraAdicionar);
            await _geralPersist.SaveChangesAsync();
            return await GetTurmaById(model.turmaId);
        }
        catch (Exception ex){
            throw new Exception(ex.Message);
    }}
    public async Task<CsvImportResultDTO> ImportarAlunosAsync( int turmaId, CsvImportRequestDTO dto)
    {
        var resultado = new CsvImportResultDTO();
        var delimiter = CSV.DetectarDelimitador(dto.Arquivo.OpenReadStream());

        var config = new CsvConfiguration(new CultureInfo("pt-BR")){
            HasHeaderRecord = true,
            Delimiter = delimiter,
            TrimOptions = TrimOptions.Trim,
            IgnoreBlankLines = true,
            MissingFieldFound = null,
            BadDataFound = null,
            HeaderValidated = null
        };
        using var stream = dto.Arquivo.OpenReadStream();
        var encoding = CSV.EncodingDetector(stream);
        using var reader = new StreamReader(stream, encoding, detectEncodingFromByteOrderMarks: true);
        using var csv = new CsvReader(reader, config);
        await csv.ReadAsync();
        csv.ReadHeader();
        var headers = csv.HeaderRecord;
        foreach (var h in headers){
            Console.WriteLine($"HEADER: '{h}'");
        }
        if (!headers.Any(h =>CSV.Normalizar(h) == CSV.Normalizar(dto.ColunaNome))){
            throw new Exception($"Coluna '{dto.ColunaNome}' não encontrada.");
        };
        int linha = 1;
        while (await csv.ReadAsync()){
            linha++;
            resultado.Total++;
            var nome = csv.GetField(dto.ColunaNome);
            try{
                Console.WriteLine(nome);
                if (string.IsNullOrWhiteSpace(nome) )
                    throw new Exception("Nome vazio");
                var aluno = new Aluno{
                    Nome = nome
                };
                _geralPersist.Add(aluno);
                await _geralPersist.SaveChangesAsync();
                await AddTurmaAluno(aluno.Id, turmaId);
                resultado.Sucesso++;
            }
            catch (Exception ex){
                resultado.Falhas++;
                resultado.Erros.Add(new CsvImportErrorDTO{
                    Linha = linha,
                    Nome = nome,
                    Erro = ex.Message
                });
        }}
        await _geralPersist.SaveChangesAsync();
        return resultado;
    }
    #endregion
    #region update
    public async Task<TurmaDTO> Update(int turmaId, TurmaDTO model){
        try{
            var turma = await _turmaPersist.GetTurmaIdAsync(turmaId);
            if (turma == null) return null;
            model.Id = turma.Id;
            _mapper.Map(model, turma);
            _geralPersist.Update(turma);
            if (await _geralPersist.SaveChangesAsync()){
                var TurmaRetorno = await _turmaPersist.GetTurmaIdAsync(turmaId);
                return _mapper.Map<TurmaDTO>(TurmaRetorno);
            }
            return null;
        }
        catch (Exception ex){
            throw new Exception(ex.Message);
    }}
    #endregion
}
