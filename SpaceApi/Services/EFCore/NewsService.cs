using AutoMapper;
using Entities.ModelDTO;
using Entities.Models;
using Repository.Contracts;
using Services.Contracts;

namespace Services;

public class NewsService:INewsService
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public NewsService(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<IEnumerable<News>> GetAllNews(bool trackChanges)
    {
        var news = await _repository.News.GenericRead(trackChanges);
        return news;
    }

    public async Task<News> GetNews(int id, bool trackChanges)
    {
        var news = _repository.News.GetNews(id, false);
        return news;
    }

    public async Task<News> CreateNews(NewsDto newsDto)
    {
        var news=_mapper.Map<News>(newsDto);
        await _repository.News.GenericCreate(news);
        _repository.Save();
        return news;
    }

    public void UpdateNews(NewsDto newsDto)
    {
        var upNews = _repository.News.GetNews(newsDto.Id, false);
        if (upNews!=null)
        {
            var newsMap= _mapper.Map<News>(newsDto);
            _repository.News.GenericUpdate(newsMap);
            _repository.Save();
        }
    }

    public void DeleteNews(int id)
    {
        var delNews = _repository.News.GetNews(id, false);
        if (delNews!=null)
        {
            _repository.News.GenericDelete(delNews);
            _repository.Save();
        }
    }
}