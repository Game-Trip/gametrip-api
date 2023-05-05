using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace GameTrip.Domain.Interfaces;

public interface IGenericRepository<T> where T : class
{
    T GetById(int id);

    IEnumerable<T> GetAll([Optional] int limit);
    Task<IEnumerable<T>> GetAllAsync([Optional] int limit);

    IEnumerable<T> Find(Expression<Func<T, bool>> expression);

    void Add(T entity);

    void AddRange(IEnumerable<T> entities);

    void Remove(T entity);

    void RemoveRange(IEnumerable<T> entities);
}