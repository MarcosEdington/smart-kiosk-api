using Microsoft.AspNetCore.Mvc;
using smart_kiosk_api.Models;
using smart_kiosk_api.Repositories; // Adicione esta linha


namespace smart_kiosk_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly RepositorioJson<Usuario> _repo = new("usuarios");

        [HttpGet]
        public ActionResult<IEnumerable<Usuario>> Get() => Ok(_repo.Carregar());

        [HttpGet("{id}")]
        public ActionResult<Usuario> Get(int id)
        {
            var user = _repo.Carregar().FirstOrDefault(u => u.Id == id);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpPost]
        public ActionResult Post(Usuario usuario)
        {
            var usuarios = _repo.Carregar();
            usuario.Id = usuarios.Any() ? usuarios.Max(u => u.Id) + 1 : 1;
            usuarios.Add(usuario);
            _repo.Salvar(usuarios);
            return CreatedAtAction(nameof(Get), new { id = usuario.Id }, usuario);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, Usuario usuarioAtualizado)
        {
            var usuarios = _repo.Carregar();
            var index = usuarios.FindIndex(u => u.Id == id);
            if (index == -1) return NotFound();

            usuarioAtualizado.Id = id; // Garante que o ID não mude
            usuarios[index] = usuarioAtualizado;
            _repo.Salvar(usuarios);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var usuarios = _repo.Carregar();
            var user = usuarios.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound();
            usuarios.Remove(user);
            _repo.Salvar(usuarios);
            return NoContent();
        }
    }
}
