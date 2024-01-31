using Entities.ModelDTO;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace Presentations.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AxiomsController:ControllerBase
{
    private readonly IAxiomsService _axioms;

    public AxiomsController(IAxiomsService axioms)
    {
        _axioms = axioms;
    }

    [HttpPost("add-Axioms")]
    public async Task<IActionResult> CreateAxioms(AxiomsDto axiomsDto)
    {
        await _axioms.CreateAxioms(axiomsDto);
        return Ok();
    }
    [HttpGet("get-all-Axioms")]
    public async Task<IActionResult> GetAllAxioms()
    {
        var geAxioms = await _axioms.GetAllAxioms(false);
        if (geAxioms==null)
        {
            return BadRequest("Hata Var");
        }
        return Ok(geAxioms);
    }
    
    [HttpPut("update-Axioms")]
    public IActionResult AxiomsUpdate(AxiomsDto axiomsDto)
    {
        _axioms.UpdateAxioms(axiomsDto);
        return Ok();
    }

    [HttpDelete("delete-Axioms")]
    public IActionResult DeleteAxioms(int id)
    {
        if (id==null)
        {
            return NotFound("kullanıcı yok");
        }
        _axioms.DeleteAxioms(id);
        return Ok();
    }

    [HttpGet("get-Axioms")]
    public IActionResult GetAxioms(int id)
    {
        var axiomsGet = _axioms.GetAxioms(id, false);
        if (axiomsGet==null)
        {
            return BadRequest("hata var");
        }
        return Ok(axiomsGet);
    }
}