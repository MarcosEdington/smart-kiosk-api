using System.Text.Json;

namespace smart_kiosk_api.Repositories
{
    public class RepositorioJson<T> where T : class
    {
        private readonly string _arquivo;

        public RepositorioJson(string nomeArquivo)
        {
            // Isso cria o arquivo JSON na mesma pasta onde a API está rodando
            _arquivo = Path.Combine(Directory.GetCurrentDirectory(), $"{nomeArquivo}.json");

            if (!File.Exists(_arquivo))
            {
                Salvar(new List<T>()); // Se não existir, cria o arquivo com uma lista vazia []
            }
        }

        public List<T> Carregar()
        {
            try
            {
                var json = File.ReadAllText(_arquivo);
                return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
            }
            catch
            {
                return new List<T>();
            }
        }

        public void Salvar(List<T> dados)
        {
            var json = JsonSerializer.Serialize(dados, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_arquivo, json);
        }
    }
}