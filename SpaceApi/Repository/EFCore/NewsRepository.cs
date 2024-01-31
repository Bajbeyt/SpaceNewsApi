using Entities.Models;
using Repository.Contracts;

namespace Repository.EFCore;

public class NewsRepository:RepositoryBase<News>,INewsRepository
{
    public NewsRepository(RepositoryContext context) : base(context)
    {
    }

    public News GetNews(int id, bool trackChanges)
    {
        return GenericReadExpression(trackChanges, x => x.Id.Equals(id)).SingleOrDefault();
    }
}