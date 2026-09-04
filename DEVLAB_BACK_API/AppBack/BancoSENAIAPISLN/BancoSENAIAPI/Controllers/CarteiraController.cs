using BancoSENAIAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BancoSENAIAPI.Controllers
{
    [Route("api/v1/[controller]")] // Rota ajustada para bater com o JavaScript
    [ApiController]
    public class CarteiraController : ControllerBase
    {
        private static List<Carteira> _carteiras = new List<Carteira>();

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_carteiras);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Carteira novaCarteira)
        {
            if (_carteiras.Any(c => c.NumeroCarteira == novaCarteira.NumeroCarteira))
            {
                return BadRequest("Já existe uma carteira cadastrada com este número.");
            }

            if (novaCarteira.ApetiteCarteira < 0)
            {
                return BadRequest("O valor do apetite deve ser maior ou igual a zero.");
            }

            _carteiras.Add(novaCarteira);
            return CreatedAtAction(nameof(Get), new { id = novaCarteira.NumeroCarteira }, novaCarteira);
        }

        [HttpPut("{numeroCarteira}")]
        public IActionResult Put(int numeroCarteira, [FromBody] Carteira carteiraAtualizada)
        {
            var carteiraExistente = _carteiras.FirstOrDefault(c => c.NumeroCarteira == numeroCarteira);
            if (carteiraExistente == null)
            {
                return NotFound("Carteira não encontrada.");
            }

            if (carteiraAtualizada.ApetiteCarteira < 0)
            {
                return BadRequest("O valor do apetite deve ser maior ou igual a zero.");
            }

            carteiraExistente.NomeCarteira = carteiraAtualizada.NomeCarteira;
            carteiraExistente.ApetiteCarteira = carteiraAtualizada.ApetiteCarteira;

            return NoContent();
        }

        [HttpDelete("{numeroCarteira}")]
        public IActionResult Delete(int numeroCarteira)
        {
            var carteira = _carteiras.FirstOrDefault(c => c.NumeroCarteira == numeroCarteira);
            if (carteira == null)
            {
                return NotFound("Carteira não encontrada.");
            }

            _carteiras.Remove(carteira);
            return NoContent();
        }
    }
}