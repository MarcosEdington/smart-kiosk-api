//using smart_kiosk_api.Services;

//var builder = WebApplication.CreateBuilder(args);

//// --- CONFIGURAÇÃO DOS SERVIÇOS (DI) ---

//builder.Services.AddControllers();


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
//            policy.AllowAnyOrigin()   
//                  .AllowAnyHeader()
//                  .AllowAnyMethod();
//        });
//});

//var app = builder.Build();

//// --- CONFIGURAÇÃO DO PIPELINE HTTP ---


//app.UseSwagger();
//app.UseSwaggerUI();


//app.UseHttpsRedirection();

//// ATIVA O CORS (Deve vir antes do MapControllers)
//app.UseCors("AllowSmartKiosk");

//app.UseAuthorization();

//app.MapControllers();

//app.Run();

using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options => {
    options.AddPolicy("LiberarReact", policy => {
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


string rootPath = Directory.GetCurrentDirectory();
string[] caminhosParaTestar = {
    Path.Combine(rootPath, "smart-kiosk-api", "wwwroot", "videos"),
    Path.Combine(rootPath, "wwwroot", "videos"),
    "/app/smart-kiosk-api/wwwroot/videos" 
};

string finalVideosPath = "";
foreach (var caminho in caminhosParaTestar) {
    if (Directory.Exists(caminho)) {
        finalVideosPath = caminho;
        break;
    }
}


if (string.IsNullOrEmpty(finalVideosPath)) {
    finalVideosPath = Path.Combine(rootPath, "wwwroot", "videos");
    Directory.CreateDirectory(finalVideosPath);
}


app.UseStaticFiles(new StaticFileOptions {
    FileProvider = new PhysicalFileProvider(finalVideosPath),
    RequestPath = "/videos",
    ServeUnknownFileTypes = true,
    DefaultContentType = "video/mp4"
});
// ---------------------------------------------------

app.UseAuthorization();
app.MapControllers();
app.Run();
