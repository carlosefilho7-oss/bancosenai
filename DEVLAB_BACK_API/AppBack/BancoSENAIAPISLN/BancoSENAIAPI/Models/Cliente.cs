using System.ComponentModel.DataAnnotations;

namespace BancoSENAIAPI.Models
{
    public class Cliente
    {
        [Key]
        public int Codigo { get; set; }

        [Required(ErrorMessage = "O Nome do cliente é obrigatório.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O CPF é obrigatório.")] // 11 digítos
        public string CPF { get; set; } = string.Empty;
        [Required]
        public int NumeroAgencia { get; set; } = 10;
        [Required]
        public decimal Saldo { get; set; } = 0.0m;
        
       
       
    }
}