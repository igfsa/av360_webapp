using Persistence.Context;
using Persistence.Contracts;
using Domain;

namespace Persistence;

public class GeralPersist : IGeralPersist
{
    private readonly APIContext _context;
    public GeralPersist(APIContext context)
    {
        _context = context;

    }
    public void Add<T>(T entity) where T : class
    {
        _context.AddAsync(entity);
    }

    public void Update<T>(T entity) where T : class
    {
        _context.Update(entity);
    }

    public void Delete<T>(T entity) where T : class
    {
        _context.Remove(entity);
    }

    public void DeleteRange<T>(T[] entityArray) where T : class
    {
        _context.RemoveRange(entityArray);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return (await _context.SaveChangesAsync()) > 0;
    }
}
