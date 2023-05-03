using System.Linq.Expressions;

namespace GameTrip.Domain.Interfaces;

public interface IGenericRepository<T> where T : class
{
    T GetById(int id);

    IEnumerable<T> GetAll();
    Task<IEnumerable<T>> GetAllAsync();

    IEnumerable<T> Find(Expression<Func<T, bool>> expression);

    void Add(T entity);

    void AddRange(IEnumerable<T> entities);

    void Remove(T entity);

    void RemoveRange(IEnumerable<T> entities);
}