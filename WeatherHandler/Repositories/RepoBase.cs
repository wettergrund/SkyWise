using WeatherHandler.Data;

namespace WeatherHandler.Repositories;

public class RepoBase<T>(WxDbContext context) : IRepoBase<T>
    where T : class
{
    protected WxDbContext _db = context;
    
    public async Task Add(T entity)
    {
        await _db.Set<T>().AddAsync(entity);
    }

    public void Delete(T entity)
    {
        _db.Set<T>().Remove(entity);
    }

    public IEnumerable<T> GetAll()
    {
        return _db.Set<T>().ToList();
    }

    public async Task<T> GetById(int id)
    {
        return await _db.Set<T>().FindAsync(id);
    }


    public async Task SaveChanges()
    {
        await _db.SaveChangesAsync();
    }

    public void Update(T entity)
    {
        _db.Set<T>().Update(entity);
    }
    
    
}