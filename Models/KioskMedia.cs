using System.ComponentModel.DataAnnotations;

namespace smart_kiosk_api.Models
{
    public class KioskMedia
    {
        public int Id { get; set; } // ID numérico para controle da API

        [Required]
        public string Chave { get; set; } = string.Empty; 

        [Required]
        public string Fonte { get; set; } = string.Empty; 

        [Required]
        public string Tipo { get; set; } = "video";      // "video" ou "iframe"

        public int Duracao { get; set; }                  // ms (20000 para iframes)

        public int Posicao { get; set; }                  // Ordem de exibição

        public bool Ativo { get; set; } = true;
    }
}
