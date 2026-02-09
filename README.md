⚙️ Smart Kiosk API - Backend Engine
Esta é a API de alta performance desenvolvida em C# .NET 8 responsável por servir os dados do Kiosk e gerenciar o armazenamento físico de mídias. O sistema utiliza um modelo de persistência híbrido entre arquivos JSON e diretórios de mídias dinâmicas.

🚀 Arquitetura e Fluxo de Dados
A API atua como o núcleo do ecossistema, realizando:

Gestão de Playlist: CRUD completo de itens de mídia salvos em Playlist.json.

Servidor de Mídia: Entrega otimizada de vídeos MP4 via middlewares de arquivos estáticos.

Lógica de Negócio Financeira: Processamento de saude financeira familiar com geração de alertas estratégicos.

🛠️ Desafios Técnicos Superados
Resgate de Diretórios no Render (Linux)
Um dos principais desafios foi garantir que a pasta wwwroot/videos fosse acessível após o deploy. Implementamos uma lógica de Localizador Automático de Diretórios que mapeia o caminho físico correto no ambiente de produção do Render, evitando erros de DirectoryNotFound.

Persistência Ágil
Desenvolvimento de uma camada de persistência customizada via serialização JSON, garantindo integridade de dados e deleção em cascata sem a necessidade de um motor SQL pesado, otimizando o custo de hospedagem e a velocidade de resposta.

📡 Endpoints Principais
GET /api/Playlist - Retorna a playlist completa e ordenada.

POST /api/Playlist/upload - Recebe arquivos MP4 e os armazena no servidor.

POST /api/Playlist/item - Adiciona ou atualiza metadados de uma mídia no JSON.

DELETE /api/Playlist/{id} - Remove registros e gerencia a reordenação das posições.

🔧 Configuração de Deploy (Render)
Para que a API funcione corretamente no Render, o projeto foi configurado com:

Runtime: .NET 8

Build Command: dotnet publish -c Release -o out

Start Command: dotnet out/smart-kiosk-api.dll

CORS: Liberado para integração com o domínio do Netlify.

📂 Estrutura de Pastas
Plaintext
/
├── Controllers/         # Endpoints da API
├── Services/            # Lógica de processamento e saúde financeira
├── wwwroot/
│   └── videos/          # Repositório físico dos vídeos MP4
└── Data/
    └── Playlist.json    # Banco de dados documental
