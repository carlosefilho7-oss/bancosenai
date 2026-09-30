using BancoSENAIAPI.Models;
using Microsoft.AspNetCore.Mvc;
using BancoSENAIAPI.Data;
using Microsoft.EntityFrameworkCore;
namespace BancoSENAIAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClienteController(AppDbContext context)
        {
            _context=context;
        }

        private static int _proximoId = 1;

        [HttpGet]
        public async  Task<IActionResult> Get()
        {
            var cliente = await _context.Cliente.ToListAsync();
            return Ok(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Cliente novoCliente)
        {
            if (await _context.Cliente.AnyAsync(a=> a.Codigo == novoCliente.Codigo))
            {
                return BadRequest("Nome e CPF são obrigatórios.");
            }

            

            _context.Cliente.Add(novoCliente);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = novoCliente.Codigo }, novoCliente);
        }

        [HttpPut("{Codigo}")]
        public async  Task <IActionResult> Put(int Codigo, [FromBody] Cliente clienteAtualizado)
        {
            var clienteExistente = await _context.Cliente.FirstOrDefaultAsync(c => c.Codigo == Codigo);
            if (clienteExistente == null)
            {
                return NotFound("Cliente não encontrado.");
            }

            if (string.IsNullOrWhiteSpace(clienteAtualizado.Nome) || string.IsNullOrWhiteSpace(clienteAtualizado.CPF))
            {
                return BadRequest("Nome e CPF são obrigatórios.");
            }
            clienteExistente.Codigo = clienteAtualizado.Codigo;
            clienteExistente.Nome = clienteAtualizado.Nome;
            clienteExistente.CPF = clienteAtualizado.CPF;
            clienteExistente.Saldo = clienteAtualizado.Saldo;
            clienteExistente.NumeroAgencia = clienteAtualizado.NumeroAgencia;
           
           
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{Codigo}")]
        public async Task<IActionResult> Delete(int Codigo)
        {
            var cliente = await _context.Cliente.FirstOrDefaultAsync(a=> a.Codigo == Codigo);
            if (cliente == null) {
                return NotFound();
                    };
            _context.Cliente.Remove(cliente);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}