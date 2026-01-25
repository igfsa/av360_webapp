namespace Persistence.Contracts;
public interface IGeralPersist
{
    //GERAL
    void Add<T>(T entity) where T : class;
    void AddRangeAsync<T>(IEnumerable<T> entity) where T : class;
    void Update<T>(T entity) where T : class;
    void Delete<T>(T entity) where T : class;
    void DeleteRange<T>(IEnumerable<T> entity) where T : class;
    Task<bool> SaveChangesAsync();
}