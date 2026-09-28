using BancoSENAIAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BancoSENAIAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AgenciaController : ControllerBase
    {
        private static List<Agencia> _agencias = new List<Agencia>
        {
            new Agencia { NumeroAgencia = 1001, Cidade = "Aracaju", SiglaEstado = "SE" },
            new Agencia { NumeroAgencia = 2002, Cidade = "São Paulo", SiglaEstado = "SP" },
            new Agencia { NumeroAgencia = 3003, Cidade = "Salvador", SiglaEstado = "BA" }
        };

        [HttpGet]
        public IActionResult ListarTodas()
        {
            return Ok(_agencias);
        }

        [HttpPost]
        public IActionResult Cadastrar([FromBody] Agencia novaAgencia)
        {

            if (_agencias.Any(a => a.NumeroAgencia == novaAgencia.NumeroAgencia))
                return BadRequest(new { message = "Este número de agência já existe." });

            _agencias.Add(novaAgencia);

            return Created("", novaAgencia);
        }

        [HttpGet("{codigo}")]
        public IActionResult ConsultarPorCodigo(int codigo)
        {
            var agencia = _agencias.FirstOrDefault(a => a.NumeroAgencia == codigo);

            if (agencia == null)
                return NotFound(new { message = "Agência não encontrada." });

            return Ok(agencia);
        }

        [HttpPut("{codigo}")]
        public IActionResult Alterar(int codigo, [FromBody] Agencia agenciaAtualizada)
        {
            var agenciaExistente = _agencias.FirstOrDefault(a => a.NumeroAgencia == codigo);

            if (agenciaExistente == null) return NotFound();

            agenciaExistente.Cidade = agenciaAtualizada.Cidade;
            agenciaExistente.SiglaEstado = agenciaAtualizada.SiglaEstado;


            return NoContent();
        }

        [HttpDelete("{codigo}")]
        public IActionResult Excluir(int codigo)
        {
            var agencia = _agencias.FirstOrDefault(a => a.NumeroAgencia == codigo);

            if (agencia == null) return NotFound();

            _agencias.Remove(agencia);
            return Ok(new { message = "Agência excluída com sucesso." });
        }
    }
}