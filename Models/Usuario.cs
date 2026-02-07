using System.ComponentModel.DataAnnotations;

namespace smart_kiosk_api.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string CPF { get; set; } = string.Empty;

        public string Telefone { get; set; } = string.Empty;

        [Required]
        public string Senha { get; set; } = string.Empty;

        public bool Ativo { get; set; } = true;
    }
}