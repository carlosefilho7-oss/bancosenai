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
        public async  Task<IActionResult> Post([FromBody] Cliente novoCliente)
        {
            if (string.IsNullOrWhiteSpace(novoCliente.NomeCliente) || string.IsNullOrWhiteSpace(novoCliente.CPF))
            {
                return BadRequest("Nome e CPF são obrigatórios.");
            }

            novoCliente.CodigoCliente = _proximoId++;
            if (novoCliente.NumeroAgencia == 0) novoCliente.NumeroAgencia = 10;

            _context.Cliente.Add(novoCliente);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = novoCliente.CodigoCliente }, novoCliente);
        }

        [HttpPut("{codigoCliente}")]
        public async  Task <IActionResult> Put(int codigoCliente, [FromBody] Cliente clienteAtualizado)
        {
            var clienteExistente = await _context.Cliente.FirstOrDefaultAsync(c => c.CodigoCliente == codigoCliente);
            if (clienteExistente == null)
            {
                return NotFound("Cliente não encontrado.");
            }

            if (string.IsNullOrWhiteSpace(clienteAtualizado.NomeCliente) || string.IsNullOrWhiteSpace(clienteAtualizado.CPF))
            {
                return BadRequest("Nome e CPF são obrigatórios.");
            }

            clienteExistente.NomeCliente = clienteAtualizado.NomeCliente;
            clienteExistente.CPF = clienteAtualizado.CPF;
            clienteExistente.DataNascimento = clienteAtualizado.DataNascimento;
            clienteExistente.Sexo = clienteAtualizado.Sexo;
            clienteExistente.Endereco = clienteAtualizado.Endereco;
            clienteExistente.Cidade = clienteAtualizado.Cidade;
            clienteExistente.Estado = clienteAtualizado.Estado;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{codigoCliente}")]
        public async Task<IActionResult> Delete(int codigoCliente)
        {
            var cliente = await _context.Carteira.FirstOrDefaultAsync(a=> a.NumeroCarteira == codigoCliente);
            if (cliente == null) {
                return NotFound();
                    };
            _context.Carteira.Remove(cliente);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}