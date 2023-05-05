using GameTrip.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace GameTrip.EFCore.Repository;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly GameTripContext _context;

    public GenericRepository(GameTripContext context) => _context = context;

    public void Add(T entity) => _context.Set<T>().Add(entity);

    public void AddRange(IEnumerable<T> entities) => _context.Set<T>().AddRange(entities);

    public IEnumerable<T> Find(Expression<Func<T, bool>> expression) => _context.Set<T>().Where(expression);

    public IEnumerable<T> GetAll([Optional] int limit) => limit is 0 ? _context.Set<T>().ToList() : _context.Set<T>().Take(limit).ToList();
    public async Task<IEnumerable<T>> GetAllAsync([Optional] int limit) => limit is 0 ? await _context.Set<T>().ToListAsync() : await _context.Set<T>().Take(limit).ToListAsync();

    public T GetById(int id) => _context.Set<T>().Find(id);
    public void Remove(T entity) => _context.Set<T>().Remove(entity);

    public void RemoveRange(IEnumerable<T> entities) => _context.Set<T>().RemoveRange(entities);
}