using System.ComponentModel.DataAnnotations;

namespace BancoSENAIAPI.Models
{
    public class Cliente
    {
        [Key]
        public int Codigo { get; set; }

        [Required]
        public string Nome { get; set; } = string.Empty;

        [Required] // 11 digítos
        public string CPF { get; set; } = string.Empty;
        [Required]
        public int NumeroAgencia { get; set; } = 10;
        [Required]
        public decimal Saldo { get; set; } = 0.0m;
        
       
       
    }
}