using AutoMapper;

using Application.Contracts;
using Application.DTOs;
using Domain.Entities;
using Persistence.Contracts;

namespace Application.Services;

public class AlunoService : IAlunoService
{
    private readonly IGeralPersist _geralPersist;
    private IAlunoPersist _alunoPersist;
    private IAlunoTurmaPersist _alunoTurmaPersist;
    private IAlunoGrupoPersist _alunoGrupoPersist;
    private readonly IMapper _mapper;

    public AlunoService(IGeralPersist geralPersist,
                        IAlunoPersist alunoPersist,
                        IAlunoTurmaPersist alunoTurmaPersist,
                        IAlunoGrupoPersist alunoGrupoPersist, 
                        IMapper mapper)
    {
        _geralPersist = geralPersist;
        _alunoPersist = alunoPersist;
        _alunoTurmaPersist = alunoTurmaPersist;
        _alunoGrupoPersist = alunoGrupoPersist;
        _mapper = mapper;
    }

    #region get
    public async Task<IEnumerable<AlunoDTO>> GetAlunos(){
        try{
            var alunos = await _alunoPersist.GetAllAlunosAsync();
            if (alunos == null) 
                return null;
            return _mapper.Map<IEnumerable<AlunoDTO>>(alunos);
        }
        catch (Exception ex){
            throw new Exception(ex.Message);
    }}

    public async Task<AlunoDTO> GetAlunoById(int Id){
        try {
            var aluno = await _alunoPersist.GetAlunoIdAsync(Id);
            if (aluno == null) 
                return null;
            return _mapper.Map<AlunoDTO>(aluno);
        }
        catch (Exception ex) {
            throw new Exception(ex.Message);
    }}

    public async Task<IEnumerable<AlunoDTO>> GetAlunosTurma(int turmaId){
        try {
            var alunos = await _alunoTurmaPersist.GetAlunosTurmaIdAsync(turmaId);
            if (alunos == null) 
                return null;
                
            return _mapper.Map<IEnumerable<AlunoDTO>>(alunos);
        }
        catch (Exception ex) {
            throw new Exception(ex.Message);
    }}
    public async Task<IEnumerable<AlunoDTO>> GetAlunosGrupo(int grupoId){
        try {
            var alunos = await _alunoGrupoPersist.GetAlunosGrupoId(grupoId);
            if (alunos == null) 
                return null;

            return _mapper.Map<IEnumerable<AlunoDTO>>(alunos);
        }
        catch (Exception ex) {
            throw new Exception(ex.Message);
    }}
    #endregion
    #region add
    public async Task<AlunoDTO> Add(AlunoDTO model) {
        try {
            var aluno = _mapper.Map<Aluno>(model);

            _geralPersist.Add(aluno);

            if (await _geralPersist.SaveChangesAsync()) {
                var alunoRetorno = await _alunoPersist.GetAlunoIdAsync(aluno.Id);

                return _mapper.Map<AlunoDTO>(alunoRetorno);
            }
            return null;
        }
        catch (Exception ex) {
            throw new Exception(ex.Message);
    }}
    #endregion
    #region update
    public async Task<AlunoDTO> Update(int alunoId, AlunoDTO model) {
        try {
            var aluno = await _alunoPersist.GetAlunoIdAsync(alunoId);
            if (aluno == null) 
                return null;

            model.Id = aluno.Id;
            _mapper.Map(model, aluno);
            _geralPersist.Update(aluno);
            if (await _geralPersist.SaveChangesAsync()) {
                var alunoRetorno = await _alunoPersist.GetAlunoIdAsync(alunoId);

                return _mapper.Map<AlunoDTO>(alunoRetorno);
            }
            return null;
        }
        catch (Exception ex) {
            throw new Exception(ex.Message);
    }}
    #endregion
}
