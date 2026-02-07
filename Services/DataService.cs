using System.Text.Json;
using smart_kiosk_api.Models;

namespace smart_kiosk_api.Services
{
    public class DataService
    {
        private readonly string _caminhoUsuarios = "usuarios.json";
        private readonly string _caminhoPlaylist = "playlist.json";

        // --- MÉTODOS PARA USUÁRIOS ---
        public List<Usuario> ObterUsuarios()
        {
            if (!File.Exists(_caminhoUsuarios)) return new List<Usuario>();
            var json = File.ReadAllText(_caminhoUsuarios);
            return JsonSerializer.Deserialize<List<Usuario>>(json) ?? new List<Usuario>();
        }

        public void SalvarUsuarios(List<Usuario> usuarios)
        {
            var json = JsonSerializer.Serialize(usuarios, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_caminhoUsuarios, json);
        }

        // --- MÉTODOS PARA PLAYLIST (O CORAÇÃO DO SMARTKIOSK) ---
        public List<KioskMedia> ObterPlaylist()
        {
            if (!File.Exists(_caminhoPlaylist)) return new List<KioskMedia>();
            var json = File.ReadAllText(_caminhoPlaylist);
            return JsonSerializer.Deserialize<List<KioskMedia>>(json) ?? new List<KioskMedia>();
        }

        public void SalvarPlaylist(List<KioskMedia> playlist)
        {
            // Ordenamos por posição antes de salvar para garantir a sequência correta no elevador
            var listaOrdenada = playlist.OrderBy(p => p.Posicao).ToList();
            var json = JsonSerializer.Serialize(listaOrdenada, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_caminhoPlaylist, json);
        }
    }
}