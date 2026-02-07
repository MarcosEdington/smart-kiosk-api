# Estágio de Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia tudo e restaura as dependências
COPY . ./
RUN dotnet restore

# Publica o projeto
RUN dotnet publish -c Release -o out

# Estágio de Execução
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Expõe a porta que o Render usa
EXPOSE 80
ENTRYPOINT ["dotnet", "smart-kiosk-api.dll"]