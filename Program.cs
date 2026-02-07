//using smart_kiosk_api.Services;

//var builder = WebApplication.CreateBuilder(args);

//// --- CONFIGURAÇÃO DOS SERVIÇOS (DI) ---

//builder.Services.AddControllers();

//// Adiciona Swagger para documentação e teste da API
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//// Serviço de dados para manipular o JSON da playlist
//builder.Services.AddScoped<smart_kiosk_api.Services.DataService>();

//// CONFIGURAÇÃO DO CORS: Liberando para o React e para arquivos locais (Elevador)
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowSmartKiosk",
//        policy =>
//        {
//            policy.AllowAnyOrigin()   // Permite que qualquer origem (inclusive o leme.html local) acesse
//                  .AllowAnyHeader()
//                  .AllowAnyMethod();
//        });
//});

//var app = builder.Build();

//// --- CONFIGURAÇÃO DO PIPELINE HTTP ---

//// Habilita o Swagger independente do ambiente para facilitar seus testes
//app.UseSwagger();
//app.UseSwaggerUI();

//// Importante: Se o seu leme.html estiver rodando via HTTP local, 
//// o Redirection pode causar bloqueios em alguns navegadores antigos de Kiosk.
//// Se preferir usar apenas HTTP no elevador, pode comentar a linha abaixo:
//app.UseHttpsRedirection();

//// ATIVA O CORS (Deve vir antes do MapControllers)
//app.UseCors("AllowSmartKiosk");

//app.UseAuthorization();

//app.MapControllers();

//app.Run();

using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// --- 1. CONFIGURAÇÃO DE CORS ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("LiberarReact", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registro do seu serviço de dados
builder.Services.AddScoped<smart_kiosk_api.Services.DataService>();

var app = builder.Build();

// --- 2. PIPELINE DE EXECUÇÃO ---

app.UseSwagger();
app.UseSwaggerUI();

// O CORS deve vir sempre antes dos arquivos estáticos
app.UseCors("LiberarReact");

// --- CONFIGURAÇÃO DE FORÇA BRUTA PARA ARQUIVOS ESTÁTICOS (RENDER/LINUX) ---

// Definimos o caminho absoluto para a pasta de vídeos
// Directory.GetCurrentDirectory() garante que estamos na raiz da aplicação no Render
var videosPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "videos");

// Log de segurança: Se a pasta não existir no servidor, nós a criamos agora
if (!Directory.Exists(videosPath))
{
    Directory.CreateDirectory(videosPath);
}

// Forçamos o mapeamento da URL /videos para a pasta física videosPath
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(videosPath),
    RequestPath = "/videos",
    ServeUnknownFileTypes = true, // Permite que o servidor entregue o .mp4 mesmo sem mime-type explícito
    DefaultContentType = "video/mp4" // Reforça que o conteúdo é vídeo
});

// Também habilitamos o suporte padrão para outros arquivos na wwwroot
app.UseStaticFiles(); 

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

