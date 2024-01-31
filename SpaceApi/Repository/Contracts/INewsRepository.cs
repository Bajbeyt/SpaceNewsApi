using Entities.Models;

namespace Repository.Contracts;

public interface INewsRepository:IRepositoryBase<News>
{
    News GetNews(int id, bool trackChanges);
}