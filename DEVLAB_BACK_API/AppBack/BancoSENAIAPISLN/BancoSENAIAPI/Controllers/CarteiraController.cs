using BancoSENAIAPI.Models;
using Microsoft.AspNetCore.Mvc;
using BancoSENAIAPI.Data;
using Microsoft.EntityFrameworkCore;
namespace BancoSENAIAPI.Controllers
{
    [Route("api/v1/[controller]")] // Rota ajustada para bater com o JavaScript
    [ApiController]
    public class CarteiraController : ControllerBase
    {
        private readonly AppDbContext _context;
        public  CarteiraController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async  Task<IActionResult> Get()
        {
            var carteira = await _context.Carteira.ToListAsync();
            return Ok(carteira);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Carteira novaCarteira)
        {
            if (await _context.Carteira.AnyAsync(a=> a.NumeroCarteira== novaCarteira.NumeroCarteira))
            {
                return BadRequest("Já existe uma carteira cadastrada com este número.");
            }

            if (novaCarteira.ApetiteCarteira < 0)
            {
                return BadRequest("O valor do apetite deve ser maior ou igual a zero.");
            }

            _context.Carteira.Add(novaCarteira);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = novaCarteira.NumeroCarteira }, novaCarteira);
        }

        [HttpPut("{numeroCarteira}")]
        public async Task<IActionResult> Put(int numeroCarteira, [FromBody] Carteira carteiraAtualizada)
        {
            var carteiraExistente = await _context.Carteira.FirstOrDefaultAsync(c => c.NumeroCarteira == numeroCarteira);
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
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{numeroCarteira}")]
        public async Task <IActionResult> Delete(int numeroCarteira)
        {
            var carteira = await _context.Carteira.FirstOrDefaultAsync(c => c.NumeroCarteira == numeroCarteira);
            if (carteira == null)
            {
                return NotFound("Carteira não encontrada.");
            }

            _context.Carteira.Remove(carteira);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}