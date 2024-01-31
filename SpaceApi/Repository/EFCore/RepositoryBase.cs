using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;

namespace Repository.EFCore;

public class RepositoryBase<T>:IRepositoryBase<T> where T:class //sadece T tipinin class olması gerektiğini bildirir
{
    private readonly RepositoryContext _context;

    public RepositoryBase(RepositoryContext context)
    {
        _context = context;
    }

    public async Task GenericCreate(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<IQueryable<T>> GenericRead(bool trackChanges) =>
        !trackChanges
            ? _context.Set<T>().AsNoTracking()
            : _context.Set<T>();

    public void GenericUpdate(T entity) =>
        _context.Set<T>().Update(entity);

    public void GenericDelete(T entity) =>
        _context.Set<T>().Remove(entity);
    
    public IQueryable<T> GenericReadExpression(bool trackChanges, Expression<Func<T, bool>> expression) =>
        !trackChanges ? _context.Set<T>().Where(expression).AsNoTracking()
            : _context.Set<T>().Where(expression);
    
}