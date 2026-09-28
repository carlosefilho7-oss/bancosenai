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
        [Required]
        public int NumeroAgencia { get; set; } = 10;
        [Required]
        public decimal SaldoTotal { get; set; } = 0.0m;
        [Required]
        public DateTime? DataNascimento { get; set; }
        [Required]
        public string? Sexo { get; set; }
        [Required]
        public string? Endereco { get; set; }
        [Required]
        public string? Cidade { get; set; }
        [Required]
        public string? Estado { get; set; }
    }
}