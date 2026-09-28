using BancoSENAIAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BancoSENAIAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private static List<Cliente> _clientes = new List<Cliente>();
        private static int _proximoId = 1;

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_clientes);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Cliente novoCliente)
        {
            if (string.IsNullOrWhiteSpace(novoCliente.NomeCliente) || string.IsNullOrWhiteSpace(novoCliente.CPF))
            {
                return BadRequest("Nome e CPF são obrigatórios.");
            }

            novoCliente.CodigoCliente = _proximoId++;
            if (novoCliente.NumeroAgencia == 0) novoCliente.NumeroAgencia = 10;

            _clientes.Add(novoCliente);
            return CreatedAtAction(nameof(Get), new { id = novoCliente.CodigoCliente }, novoCliente);
        }

        [HttpPut("{codigoCliente}")]
        public IActionResult Put(int codigoCliente, [FromBody] Cliente clienteAtualizado)
        {
            var clienteExistente = _clientes.FirstOrDefault(c => c.CodigoCliente == codigoCliente);
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

            return NoContent();
        }
    }
}