using Microsoft.EntityFrameworkCore;
using RepositoryPatern.Interfaces;

namespace RepositoryPatern.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly DbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(DbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

    public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

    public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

    public void Update(T entity) => _dbSet.Update(entity);

    public void Delete(T entity) => _dbSet.Remove(entity);

    // The SaveAsync method is responsible for saving changes to the database. It calls the SaveChangesAsync method on the DbContext and returns true if the number of affected rows is greater than 0, indicating that changes were successfully saved to the database.
    public async Task<bool> SaveAsync() => await _context.SaveChangesAsync() > 0;
}