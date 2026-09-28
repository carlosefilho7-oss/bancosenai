using System.ComponentModel.DataAnnotations;

namespace BancoSENAIAPI.Models
{
    public class Cliente
    {
        [Key]
        public int CodigoCliente { get; set; }

        [Required(ErrorMessage = "O Nome do cliente é obrigatório.")]
        public string NomeCliente { get; set; } = string.Empty;

        [Required(ErrorMessage = "O CPF é obrigatório.")] // 11 digítos
        public string CPF { get; set; } = string.Empty;

        public int NumeroAgencia { get; set; } = 10;

        public decimal SaldoTotal { get; set; } = 0.0m;
           
        public DateTime? DataNascimento { get; set; }
        public string? Sexo { get; set; }
        public string? Endereco { get; set; }
        public string? Cidade { get; set; }
        public string? Estado { get; set; }
    }
}