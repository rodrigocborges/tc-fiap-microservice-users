# Estágio 1: Build da Aplicação
# SDK do .NET 9 para compilar e publicar a aplicação.
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /source

# Copia os arquivos .csproj e restaura as dependências primeiro para aproveitar o cache do Docker.
COPY *.sln .
COPY FIAPCloudGames.API/*.csproj ./FIAPCloudGames.API/
COPY FIAPCloudGames.Application/*.csproj ./FIAPCloudGames.Application/
COPY FIAPCloudGames.Domain/*.csproj ./FIAPCloudGames.Domain/
COPY FIAPCloudGames.Infrastructure/*.csproj ./FIAPCloudGames.Infrastructure/
COPY FIAPCloudGames.Tests/*.csproj ./FIAPCloudGames.Tests/
RUN dotnet restore "FIAPCloudGames.sln"

# Copia todo o resto do código fonte e publica a API.
COPY . .
# O comando de publish compila o projeto e coloca os artefatos prontos para execução na pasta /app/publish.
RUN dotnet publish "FIAPCloudGames.API/FIAPCloudGames.API.csproj" -c Release -o /app/publish

# Estágio 2: Criação da Imagem Final
# Usamos a imagem ASP.NET runtime, que é muito menor que a SDK, pois só precisamos rodar a aplicação.
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .

# Define a porta que a aplicação irá expor dentro do contêiner.
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Define o comando para iniciar a aplicação quando o contêiner for executado.
ENTRYPOINT ["dotnet", "FIAPCloudGames.API.dll"]