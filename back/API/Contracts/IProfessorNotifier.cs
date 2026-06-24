namespace Application.Contracts;

public interface IProfessorNotifier
{
    Task ProfessorAtualizado(int ProfessorId);
}
