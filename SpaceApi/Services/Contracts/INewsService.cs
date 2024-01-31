using Entities.ModelDTO;
using Entities.Models;

namespace Services.Contracts;

public interface INewsService
{
    Task<IEnumerable<News>> GetAllNews(bool trackChanges);
    Task<News> GetNews(int id, bool trackChanges);
    Task<News> CreateNews(NewsDto newsDto);
    void UpdateNews(NewsDto newsDto);
    void DeleteNews(int id);
}