using Persistence.Context;
using Persistence.Contracts;

namespace Persistence;

public class GeralPersist(APIContext context) : IGeralPersist
{
    private readonly APIContext _context = context;

    public void Add<T>(T entity) where T : class
    {
        _ = _context.AddAsync(entity);
    }
    public void AddRangeAsync<T>(IEnumerable<T> entityArray) where T : class
    {
        _ = _context.AddRangeAsync(entityArray);
    }
    public void Update<T>(T entity) where T : class
    {
        _ = _context.Update(entity);
    }
    public void Delete<T>(T entity) where T : class
    {
        _ = _context.Remove(entity);
    }
    public void DeleteRange<T>(IEnumerable<T> entityArray) where T : class
    {
        _context.RemoveRange(entityArray);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return (await _context.SaveChangesAsync()) > 0;
    }
}
