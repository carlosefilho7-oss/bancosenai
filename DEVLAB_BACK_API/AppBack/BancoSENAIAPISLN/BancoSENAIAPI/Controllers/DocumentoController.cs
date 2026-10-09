using BancoSENAIAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace BancoSENAIAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class DocumentoController : Controller
    {
        private readonly string _caminhoRaiz = Path.Combine
            (Directory.GetCurrentDirectory()
            , "ClienteArquivos");

        private readonly AppDbContext _context;
        public DocumentoController(AppDbContext context)
        {
            _context = context;
        }
        private static List<Models.DocumentoMetadado> _documentoMetadados = new List<Models.DocumentoMetadado>();

        private static int _nextid = 1;

        [HttpPost("upload/{CodigoCliente}")]
        public async Task<IActionResult> AnexarArquivo(int CodigoCliente, IFormFile arquivo)
        {

            const long tamanhodocumento = 2 *(1024*1024);
            if(arquivo.Length > tamanhodocumento)
            {
                return BadRequest(new {mensagem = "arquivo com tamanho excedido"});
            }
            if(arquivo == null || arquivo.Length == 0)
            {
                return BadRequest("nenhum arquivo foi enviado");
            }
            var extensoespermitidas = new[] { ".pdf", ".jpg", ".png" };
            string extensoes = Path.GetExtension(arquivo.FileName).ToLowerInvariant();

            if (!extensoespermitidas.Contains(extensoes))
            {
                return BadRequest(new {mensagem = $"Extensão{extensoes} inválida para envio."});
            }

            string pastaCliente = Path.Combine(_caminhoRaiz, CodigoCliente.ToString());

            if (!Directory.Exists(pastaCliente))
            {
                Directory.CreateDirectory(pastaCliente);
            }

            string extensao = Path.GetExtension(arquivo.FileName);

            string nameOriginal = Path.GetFileNameWithoutExtension(arquivo.FileName);
            string novonome = $"{CodigoCliente}_{nameOriginal}_{Guid.NewGuid()}{extensao}";
            string caminhofinal = Path.Combine(pastaCliente, novonome);

            using (var stream = new FileStream(caminhofinal, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }

            var documentosMetadados = new Models.DocumentoMetadado
            {
                Id = _nextid++,
                Nome = nameOriginal,
                Extensao = extensao,
                Caminho = caminhofinal,
                CodigoCliente = CodigoCliente
            };

            _context.DocumentoMetadados.Add(documentosMetadados);
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Documento anexado com sucesso", arquivoSalvo = novonome });
        }
        [HttpGet("listar/{codigoCliente}")]
        public  IActionResult Listardocumentos()
        {
            return Ok(_context.DocumentoMetadados);
        }

        [HttpGet("download/{id}")] 
        public async  Task<IActionResult> dowload(int id) 
        {
         var doc = await _context.DocumentoMetadados.FirstOrDefaultAsync(d =>  d.Id == id);

            if (doc == null)
            {
                return NotFound("documento não encontrado");
            }
            var arquivoinfo = new System.IO.FileInfo(doc.Caminho);
            long tamanhoarquivo = 2 * 1024 * 1024;

            if(arquivoinfo.Length > tamanhoarquivo)
            {
                return BadRequest(new { mensagem = "o arquivo excede o tamanho de 2 mb para download" });
            }

            byte[] fileBytes = System.IO.File.ReadAllBytes(doc.Caminho);
            string nomedocumento = $"{doc.Nome}{doc.Extensao}";
            return File(fileBytes, "application/octet-stream",nomedocumento);
        }

        [HttpDelete("excluir/{id}")]

        public async Task<IActionResult> deletar(int id)
        {
            var doc =await _context.DocumentoMetadados.FirstOrDefaultAsync(d => d.Id == id);

            if(doc == null)
            {
                return NotFound("documento não encontrado");
            }

            _context.DocumentoMetadados.Remove(doc);
            await _context.SaveChangesAsync();
            return Ok("arquivo deletado");
        }



    }
}
