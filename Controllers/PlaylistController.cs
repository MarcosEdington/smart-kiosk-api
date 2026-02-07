using Microsoft.AspNetCore.Mvc;
using smart_kiosk_api.Models;
using smart_kiosk_api.Services;

namespace smart_kiosk_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlaylistController : ControllerBase
    {
        private readonly DataService _dataService;

        public PlaylistController(DataService dataService)
        {
            _dataService = dataService;
        }

        // GET: api/playlist (O Kiosk usa este para baixar a lista de vídeos)
        [HttpGet]
        public IActionResult Get()
        {
            var playlist = _dataService.ObterPlaylist()
                                       .Where(p => p.Ativo)
                                       .OrderBy(p => p.Posicao);
            return Ok(playlist);
        }

        // POST: api/playlist (Atualiza a ordem ou status da playlist completa)
        [HttpPost]
        public IActionResult Post([FromBody] List<KioskMedia> novaPlaylist)
        {
            _dataService.SalvarPlaylist(novaPlaylist);
            return Ok(new { message = "Playlist atualizada com sucesso!" });
        }

        // POST: api/playlist/item (Adiciona um novo item manualmente)
        [HttpPost("item")]
        public IActionResult PostItem([FromBody] KioskMedia novoItem)
        {
            var playlist = _dataService.ObterPlaylist();

            // Gera novo ID e define a última posição sequencial
            novoItem.Id = playlist.Any() ? playlist.Max(p => p.Id) + 1 : 1;
            novoItem.Posicao = playlist.Any() ? playlist.Max(p => p.Posicao) + 1 : 1;

            playlist.Add(novoItem);
            _dataService.SalvarPlaylist(playlist);

            return Ok(new { message = "Mídia adicionada com sucesso!", item = novoItem });
        }

        // POST: api/playlist/upload (Processa o arquivo de vídeo e salva na wwwroot)
        [HttpPost("upload")]
        public async Task<IActionResult> UploadVideo(IFormFile videoFile, [FromForm] string chave)
        {
            if (videoFile == null || videoFile.Length == 0)
                return BadRequest("Arquivo não selecionado.");

            // Define o caminho físico da pasta wwwroot/videos de forma compatível (Windows/Linux)
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "videos");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            // Normaliza o nome do arquivo usando a chave (ex: Video Da Promo -> video_da_promo.mp4)
            var fileName = $"{chave.Replace(" ", "_").ToLower()}.mp4";
            var filePath = Path.Combine(folderPath, fileName);

            // Salva o arquivo fisicamente no servidor
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await videoFile.CopyToAsync(stream);
            }

            // AJUSTE: Retornamos a URL com a barra inicial para o Front-end localizar na raiz da API
            var relativeUrl = $"/videos/{fileName}"; 
            
            return Ok(new { url = relativeUrl });
        }
    }
}
