using BancoSENAIAPI.Models;
using Microsoft.AspNetCore.Mvc;
using BancoSENAIAPI.Data;
using Microsoft.EntityFrameworkCore;
namespace BancoSENAIAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AgenciaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AgenciaController(AppDbContext context)
        {
            _context=context;  
        }

           

        [HttpGet]
        public async Task<IActionResult> ListarTodas()
        {
            var agencias = await _context.Agencia.ToListAsync();
            return Ok(agencias);
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] Agencia novaAgencia)
        {

            if (await _context.Agencia.AnyAsync(a=> a.NumeroAgencia== novaAgencia.NumeroAgencia))
                return BadRequest(new { message = "Este número de agência já existe." });

            _context.Agencia.Add(novaAgencia);
            await _context.SaveChangesAsync();

            return Created("", novaAgencia);
        }

        [HttpGet("{NumeroAgencia}")]
        public async  Task<IActionResult> ConsultarPorCodigo(int NumeroAgencia)
        {
            var agencia = await _context.Agencia.FirstOrDefaultAsync(a => a.NumeroAgencia == NumeroAgencia);

            if (agencia == null)
                return NotFound(new { message = "Agência não encontrada." });

            return Ok(agencia);
        }

        [HttpPut("{NumeroAgencia}")]
        public async Task<IActionResult> Alterar(int NumeroAgencia, [FromBody] Agencia agenciaAtualizada)
        {
            var agenciaExistente = await _context.Agencia.FirstOrDefaultAsync(a => a.NumeroAgencia == NumeroAgencia);

            if (agenciaExistente == null) return NotFound();

            agenciaExistente.Cidade = agenciaAtualizada.Cidade;
            agenciaExistente.SiglaEstado = agenciaAtualizada.SiglaEstado;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{NumeroAgencia}")]
        public async Task<IActionResult> Excluir(int NumeroAgencia)
        {
            var agencia = await _context.Agencia.FirstOrDefaultAsync(a => a.NumeroAgencia == NumeroAgencia);

            if (agencia == null) return NotFound();

            _context.Agencia.Remove(agencia);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Agência excluída com sucesso." });
        }
    }
}