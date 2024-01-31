using System.Threading.Tasks;
using Entities.ModelDTO;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace Presentations.Controllers;
[Route("api/[controller]")]
[ApiController]
public class NewsController:ControllerBase
{
    private readonly INewsService _news;

    public NewsController(INewsService news)
    {
        _news = news;
    }

    [HttpPost("add-News")]
    public async Task<IActionResult> AddNews(NewsDto newsDto)
    {
        await _news.CreateNews(newsDto);
        return Ok();
    }

    [HttpGet("get-all-News")]
    public async Task<IActionResult> GetAllNews()
    {
        var geNews = await _news.GetAllNews(false);
        if (geNews==null)
        {
            return BadRequest("Hata Var");
        }
        return Ok(geNews);
    }

    [HttpPut("update-News")]
    public IActionResult UpdateNews(NewsDto newsDto)
    {
        _news.UpdateNews(newsDto);
        return Ok();
    }

    [HttpDelete("delete-News")]
    public IActionResult DeleteNews(int id)
    {
        if (id==null)
        {
            return NotFound("kullanıcı yok");
        }
        _news.DeleteNews(id);
        return Ok();
    }

    [HttpGet("get-News")]
    public IActionResult GetNews(int id)
    {
        var newsGet = _news.GetNews(id, false);
        if (newsGet==null)
        {
            return BadRequest("hata var");
        }
        return Ok(newsGet);
    }
}