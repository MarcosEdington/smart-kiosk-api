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
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<smart_kiosk_api.Services.DataService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("LiberarReact");

// --- AJUSTE DE CAMINHO PARA A ESTRUTURA DO SEU GITHUB ---
// O Render executa a partir da raiz, mas seus arquivos estão em smart-kiosk-api/wwwroot
var videosPath = Path.Combine(Directory.GetCurrentDirectory(), "smart-kiosk-api", "wwwroot", "videos");

// Fallback: Se não achar com a pasta do projeto, tenta o padrão (ajuda no local/produção)
if (!Directory.Exists(videosPath))
{
    videosPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "videos");
}

if (!Directory.Exists(videosPath))
{
    Directory.CreateDirectory(videosPath);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(videosPath),
    RequestPath = "/videos",
    ServeUnknownFileTypes = true,
    DefaultContentType = "video/mp4"
});

app.UseStaticFiles(); 
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

