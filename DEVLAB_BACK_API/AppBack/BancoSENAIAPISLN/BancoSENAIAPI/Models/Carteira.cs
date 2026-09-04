using System.ComponentModel.DataAnnotations;

namespace BancoSENAIAPI.Models
{
    public class Carteira
    {
        [Key]
        [Required(ErrorMessage = "O número da carteira é obrigatório.")]
        public int NumeroCarteira { get; set; }

        [Required(ErrorMessage = "O nome da carteira é obrigatório.")] //imagina se o cara bota tipo duducard lol
        public string NomeCarteira { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "O apetite da carteira deve ser maior ou igual a zero.")]
        public decimal ApetiteCarteira { get; set; } = 1000000.00m;
    }
}