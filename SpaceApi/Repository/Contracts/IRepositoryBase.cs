using System.Linq.Expressions;
using Entities.Models;

namespace Repository.Contracts;

public interface IRepositoryBase<T>
{
    Task GenericCreate(T entity);
    Task<IQueryable<T>> GenericRead(bool trackChanges);
    void GenericUpdate(T entity);
    void GenericDelete(T entity);
    IQueryable<T> GenericReadExpression(bool trackChanges, Expression<Func<T, bool>> expression);
    //genericreadexpresssion istenen bilgilere göre verileri getirir
    //expression ise veriler arasında filtreleme işlemi yapıyor

}