using Entities;
using Entities.ModelDTO;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.EFCore;
using Services.Contracts;

namespace Presentations.Controllers;
[Route("api/[controller]")]
[ApiController]
public class RocketController:ControllerBase
{
    private readonly IServiceManager _service;

    public RocketController(IServiceManager service)
    {
        _service = service;
    }

    [HttpPost("add-Rocket")]
    public async Task<IActionResult> AddRocket(RocketDto rocketDto)
    {
        if (rocketDto == null) return BadRequest("Hata var");
        await _service.RocketService.CreateRocket(rocketDto);
        return Ok();
    }

    [HttpGet("get-all-Rocket")]
    public async Task<IActionResult> GetAllRocket()
    { 
        var rocket=await _service.RocketService.GetAllRocket(false);
        if (rocket==null)
        {
            return BadRequest("hata var");
        }

        return Ok(rocket);
    }

    [HttpPut("update-Rocket")]
    public IActionResult UpdateRocket(RocketDto rocketDto)
    {
        _service.RocketService.UpdateRocket(rocketDto);
        return Ok();
    }

    [HttpDelete("delete-Rocket")]
    public IActionResult DeleteRocket(int id)
    {
        _service.RocketService.DeleteRocket(id);
        return Ok();
    }

    [HttpGet("get-Rocket")]
    public async Task<IActionResult> GetRocket(int id)
    {
        var getRocket = await _service.RocketService.GetRockets(id, false);
        if (getRocket==null)
        {
            return BadRequest("Hata var");
        }

        return Ok(getRocket);
    }

    // // [Authorize]
    // [HttpGet("get-Rockets-list")]
    // public IActionResult GetRocketList([FromBody] RequestParameters parameters)
    // {//[FromBody] Bu özellik, MVC'ye bu parametrenin HTTP isteği gövdesinden alınması gerektiğini söyler. Özellikle JSON, XML veya diğer medya türlerinde gönderilen veriler için kullanılır.
    //     Response.Headers.Add("Content-Type","application/json"); //Bu satır, HTTP yanıtının içeriğinin JSON formatında olduğunu belirtir.
    //     Response.Headers.Add("Allow","GET");//Bu satır, bu endpoint için sadece HTTP GET isteklerinin kabul edildiğini belirtir
    //     var incomingRocketList = _service.RocketService.GetAllRocketPagination(parameters, false);
    //     if (incomingRocketList != null) return Ok(incomingRocketList);
    //     return NotFound(); 
    //     //1- Pagination Filter modelinin entity katmanında tanımlanması gerekmektedir.
    //     //2- Tanımlanan Pagination Filter modeli Data katmanındaki yada repodaki query eklenir.
    //     //3- Service tarafında Pagination Filter modeline göre düzenleme yapılır.
    //     //4- Sunum katmanındaki controller altında bulunan endpoint Pagination Filter'a göre düzenlenir.
    // }
}